using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

    /// <summary>
    /// This class contains the heart of the Master Audio API. There are also convenience methods here for Playlist Controllers, even though you can call those methods on the Playlist Controller itself as well.
    /// </summary>
public class MasterAudio : MonoBehaviour {
	public const string MasterAudioFolderPath = "Assets/DarkTonic/MasterAudio";

	public const string PREVIEW_TEXT = "*Fading & random settings are ignored by preview in edit mode.";
    
	private const string USE_DB_KEY = "~MA_UseDbScaleForVolume~";
	private const string STORED_LANGUAGE_NAME_KEY = "~MA_Language_Key~";

    public static readonly YieldInstruction endOfFrameDelay = new WaitForEndOfFrame();

    public const string GIZMO_FILE_NAME = "MasterAudio Icon.png";
    public const int HARD_CODED_BUS_OPTIONS = 2;
    public const string ALL_BUSES_NAME = "[All]";
    public const string NO_GROUP_NAME = "[None]";
    public const string DYNAMIC_GROUP_NAME = "[Type In]";
    public const string NO_PLAYLIST_NAME = "[No Playlist]";
    public const string NO_VOICE_LIMIT_NAME = "[NO LMT]";
    public const string ONLY_PLAYLIST_CONTROLLER_NAME = "~only~";
    public const float INNER_LOOP_CHECK_INTERVAL = .1f;

    #region public variables
    public MasterAudio.AudioLocation bulkLocationMode = MasterAudio.AudioLocation.Clip;
    public DragGroupMode curDragGroupMode = DragGroupMode.OneGroupPerClip;

    public bool mixerMuted = false;
    public bool playlistsMuted = false;

    public LanguageMode langMode = LanguageMode.UseDeviceSetting;
    public SystemLanguage testLanguage = SystemLanguage.English;
    public SystemLanguage defaultLanguage = SystemLanguage.English;
    public List<SystemLanguage> supportedLanguages = new List<SystemLanguage>() {
		SystemLanguage.English
	};

    public string busFilter = string.Empty;
    public bool useTextGroupFilter = false;
    public string textGroupFilter = string.Empty;
    public bool resourceClipsPauseDoNotUnload = false;
    public bool resourceClipsAllLoadAsync = true;
    public Transform playlistControllerPrefab;
    public bool persistBetweenScenes = false;
    public bool areGroupsExpanded = true;
    public Transform soundGroupTemplate;
    public Transform soundGroupVariationTemplate;
    public List<GroupBus> groupBuses = new List<GroupBus>();
    public bool groupByBus = true;
    public bool showGizmos = true;
    public bool showAdvancedSettings = true;
    public bool showLocalization = true;

    public bool playListExpanded = true;
    public bool playlistEditorExpanded = true;

    public List<Playlist> musicPlaylists = new List<Playlist>() {
		new Playlist()
	};

    public float masterAudioVolume = 1.0f;
    public bool ignoreTimeScale = false;
    public bool prioritizeOnDistance = false;
    public int rePrioritizeEverySecIndex = 0;
    public bool visualAdvancedExpanded = true;
    public bool logAdvancedExpanded = true;
    
	public bool showFadingSettings = false;
	public bool stopZeroVolumeVariations = false;
	public bool stopZeroVolumeGroups = false;
	public bool stopZeroVolumeBuses = false;
	public bool stopZeroVolumePlaylists = false;

	public bool resourceAdvancedExpanded = true;
    public bool useClipAgePriority = false;
    public bool LogSounds = false;
	public bool logCustomEvents = false;
	public bool disableLogging = false;
    public bool showMusicDucking = false;
    public bool enableMusicDucking = true;
    public List<DuckGroupInfo> musicDuckingSounds = new List<DuckGroupInfo>();
    public float defaultRiseVolStart = .5f;
    public float duckedVolumeMultiplier = .5f;
    public float crossFadeTime = 1f;
    public float masterPlaylistVolume = 1f;

    public string newEventName = "my event";
    public bool showCustomEvents = true;
    public List<CustomEvent> customEvents = new List<CustomEvent>();
    public Dictionary<string, DuckGroupInfo> duckingBySoundType = new Dictionary<string, DuckGroupInfo>(); // populated at runtime
    public int frames = 0;

    private Transform trans;
    private bool soundsLoaded;
    private bool warming = false;

    #endregion

    #region private variables

    private static Dictionary<string, AudioGroupInfo> audioSourcesBySoundType = new Dictionary<string, AudioGroupInfo>();

    private static Dictionary<string, List<int>> randomizer = new Dictionary<string, List<int>>();
	private static Dictionary<string, List<int>> randomizerLeftovers = new Dictionary<string, List<int>>();
	private static Dictionary<string, List<int>> clipsPlayedBySoundTypeOldestFirst = new Dictionary<string, List<int>>();
    private static List<MasterAudioGroup> soloedGroups = new List<MasterAudioGroup>();
    private static List<BusFadeInfo> busFades = new List<BusFadeInfo>();
    private static List<GroupFadeInfo> groupFades = new List<GroupFadeInfo>();
    private static Dictionary<ICustomEventReceiver, Transform> eventReceivers = new Dictionary<ICustomEventReceiver, Transform>();

    private static Dictionary<string, PlaylistController> playlistControllersByName = new Dictionary<string, PlaylistController>();
    private static Dictionary<string, SoundGroupRefillInfo> lastTimeSoundGroupPlayed = new Dictionary<string, SoundGroupRefillInfo>();
    private static MasterAudio _instance;
    private static AudioSource _previewerInstance;
    private static Transform _audListenerTrans;
    private static bool appIsShuttingDown;
	private static float repriTime = -1f;
    private static List<string> groupsToRemove;

    private static YieldInstruction innerLoopDelay;
    #endregion

    #region Master Audio enums
    public enum CustomEventReceiveMode {
        Always,
        WhenDistanceLessThan,
        WhenDistanceMoreThan,
        Never
    }
	
	public enum AudioLocation {
        Clip,
        ResourceFile
    }

    public enum BusCommand {
        None,
        FadeToVolume,
        Mute,
        Pause,
        Solo,
        Unmute,
        Unpause,
        Unsolo,
        Stop,
        ChangeBusPitch
    }

    public enum DragGroupMode {
        OneGroupPerClip,
        OneGroupWithVariations
    }

    public enum EventSoundFunctionType {
        PlaySound,
        GroupControl,
        BusControl,
        PlaylistControl,
        CustomEventControl,
        GlobalControl
    }

    public enum LanguageMode {
        UseDeviceSetting,
        SpecificLanguage,
        DynamicallySet
    }

    public enum PlaylistCommand {
        None,
        ChangePlaylist, // by name
        FadeToVolume,
        PlayClip, // by name
        PlayRandomSong,
        PlayNextSong,
        Pause,
        Resume,
        Stop,
        Mute,
        Unmute,
        ToggleMute,
        Restart,
		Start
    }

    public enum CustomEventCommand {
        None,
        FireEvent
    }

    public enum GlobalCommand {
        None,
        PauseMixer,
        UnpauseMixer,
        StopMixer,
        StopEverything,
        PauseEverything,
        UnpauseEverything,
        MuteEverything,
        UnmuteEverything
    }

    public enum SoundGroupCommand {
        None,
        FadeToVolume,
        FadeOutAllOfSound,
        Mute,
        Pause,
        Solo,
        StopAllOfSound,
        Unmute,
        Unpause,
        Unsolo,
        StopAllSoundsOfTransform,
        PauseAllSoundsOfTransform,
        UnpauseAllSoundsOfTransform,
        StopSoundGroupOfTransform,
        PauseSoundGroupOfTransform,
        UnpauseSoundGroupOfTransform,
        FadeOutSoundGroupOfTransform,
		RefillSoundGroupPool
    }

    public enum SongFadeInPosition {
        NewClipFromBeginning = 1,
        NewClipFromLastKnownPosition = 3,
        SynchronizeClips = 5,
    }

    public enum SoundSpawnLocationMode {
        MasterAudioLocation,
        CallerLocation,
        AttachToCaller
    }

    public enum VariationCommand {
        None = 0,
        Stop = 1,
        Pause = 2,
        Unpause = 3
    }

    public static readonly List<SoundGroupCommand> GroupCommandsWithNoGroupSelector = new List<SoundGroupCommand>() {
		SoundGroupCommand.None,
		SoundGroupCommand.PauseAllSoundsOfTransform,
		SoundGroupCommand.StopAllSoundsOfTransform,
		SoundGroupCommand.UnpauseAllSoundsOfTransform
	};

    public static readonly List<SoundGroupCommand> GroupCommandsWithNoAllGroupSelector = new List<SoundGroupCommand>() {
		SoundGroupCommand.None,
		SoundGroupCommand.FadeOutSoundGroupOfTransform,
		SoundGroupCommand.PauseSoundGroupOfTransform,
		SoundGroupCommand.UnpauseSoundGroupOfTransform,
		SoundGroupCommand.StopSoundGroupOfTransform
	};

    public static readonly List<PlaylistCommand> PlaylistCommandsThatFailOnStart = new List<PlaylistCommand>() {
		PlaylistCommand.PlayClip,
		PlaylistCommand.PlayRandomSong,
		PlaylistCommand.PlayNextSong,
		PlaylistCommand.Pause,
		PlaylistCommand.Stop
	};

    #endregion

    #region inner classes
    [Serializable]
    public class AudioGroupInfo {
        public List<AudioInfo> _sources;
        public int _lastFramePlayed;
        public float _lastTimePlayed;
        public MasterAudioGroup _group;

        public AudioGroupInfo(List<AudioInfo> sources, MasterAudioGroup groupScript) {
            this._sources = sources;
            this._lastFramePlayed = -50;
            this._lastTimePlayed = -50;
            this._group = groupScript;
        }
    }

    [Serializable]
    public class AudioInfo {
        public AudioSource source;
        public float originalVolume;
        public float lastPercentageVolume;
        public float lastRandomVolume;
        public SoundGroupVariation variation;

        public AudioInfo(SoundGroupVariation _variation, AudioSource _source, float _origVol) {
            this.variation = _variation;
            this.source = _source;
            this.originalVolume = _origVol;
            this.lastPercentageVolume = 1f;
            this.lastRandomVolume = 0f;
        }
    }

    [Serializable]
    public class Playlist {
        public bool isExpanded = true;
        public string playlistName = "new playlist";
        public SongFadeInPosition songTransitionType = SongFadeInPosition.NewClipFromBeginning;
        public List<MusicSetting> MusicSettings;
        public MasterAudio.AudioLocation bulkLocationMode = MasterAudio.AudioLocation.Clip;
        public CrossfadeTimeMode crossfadeMode = CrossfadeTimeMode.UseMasterSetting;
        public float crossFadeTime = 1f;
        public bool fadeInFirstSong = false;
        public bool fadeOutLastSong = false;
        public bool resourceClipsAllLoadAsync = true;

        public enum CrossfadeTimeMode {
            UseMasterSetting,
            Override
        }

        public Playlist() {
            MusicSettings = new List<MusicSetting>();
        }
    }

    [Serializable]
    public class SoundGroupRefillInfo {
        public float _lastTimePlayed;
        public float _inactivePeriodSeconds;

        public SoundGroupRefillInfo(float lastTimePlayed, float inactivePeriodSeconds) {
            _lastTimePlayed = lastTimePlayed;
            _inactivePeriodSeconds = inactivePeriodSeconds;
        }
    }

    #endregion

    #region MonoDevelop events and Helpers
    void Awake() {
        if (GameObject.FindObjectsOfType(typeof(MasterAudio)).Length > 1) {
            Destroy(gameObject);
            Debug.Log("More than one Master Audio prefab exists in this Scene. Destroying the newer one called '" + this.name + "'. You may wish to set up a Bootstrapper Scene so this does not occur.");
            return;
        }

        this.useGUILayout = false;
        soundsLoaded = false;

        innerLoopDelay = new WaitForSeconds(INNER_LOOP_CHECK_INTERVAL);

		var aud = this.GetComponent<AudioSource>();
        if (aud != null) {
            // delete the previewer
            GameObject.Destroy(aud);
        }

        audioSourcesBySoundType.Clear();
        playlistControllersByName.Clear();
        lastTimeSoundGroupPlayed.Clear();

        var plNames = new List<string>();
        AudioResourceOptimizer.ClearAudioClips();

        PlaylistController.Instances = null; // clear the cache
        var playlists = PlaylistController.Instances;
        for (var i = 0; i < playlists.Count; i++) {
            var aList = playlists[i];

            if (plNames.Contains(aList.name)) {
                Debug.LogError("You have more than 1 Playlist Controller with the name '" + aList.name + "'. You must name them all uniquely or the same-named ones will be deleted once they awake.");
                continue;
            } else {
                plNames.Add(aList.name);
            }

            playlistControllersByName.Add(aList.name, aList);
            if (persistBetweenScenes) {
                DontDestroyOnLoad(aList);
            }
        }

        // start up Objects!
        if (persistBetweenScenes) {
            DontDestroyOnLoad(this.gameObject);
        }

        Transform parentGroup;
        List<AudioInfo> sources;

        var playedStatuses = new List<int>();
        AudioSource source;
        AudioGroupInfo _group;
        MasterAudioGroup groupScript;
        randomizer = new Dictionary<string, List<int>>();
		randomizerLeftovers = new Dictionary<string, List<int>>();
		clipsPlayedBySoundTypeOldestFirst = new Dictionary<string, List<int>>();
        string soundType = string.Empty;

        var firstGroupName = string.Empty;

        var allVars = new List<SoundGroupVariation>();

        groupsToRemove = new List<string>(Trans.childCount);

        for (var k = 0; k < Trans.childCount; k++) {
            parentGroup = Trans.GetChild(k);

            sources = new List<AudioInfo>();

            groupScript = parentGroup.GetComponent<MasterAudioGroup>();

            if (groupScript == null) {
                Debug.LogError("MasterAudio could not find 'MasterAudioGroup' script for group '" + parentGroup.name + "'. Skipping this group.");
                continue;
            }

            soundType = parentGroup.name;

            if (string.IsNullOrEmpty(firstGroupName)) {
                firstGroupName = soundType;
            }

            var newWeightedChildren = new List<Transform>();

            SoundGroupVariation variation = null;
            SoundGroupVariation childVariation = null;
            Transform child = null;

            for (int i = 0; i < parentGroup.childCount; i++) {
                child = parentGroup.GetChild(i);
                variation = child.GetComponent<SoundGroupVariation>();
				source = child.GetComponent<AudioSource>();

                var weight = variation.weight;

                for (var j = 0; j < weight; j++) {
                    if (j > 0) {
                        var extraChild = (GameObject)GameObject.Instantiate(child.gameObject, parentGroup.transform.position, Quaternion.identity);
                        extraChild.transform.name = child.gameObject.name;
                        childVariation = extraChild.GetComponent<SoundGroupVariation>();
                        childVariation.weight = 1;

                        newWeightedChildren.Add(extraChild.transform);
						source = extraChild.GetComponent<AudioSource>();

                        sources.Add(new AudioInfo(childVariation, source, source.volume));
                        allVars.Add(childVariation);

                        if (childVariation.audLocation == MasterAudio.AudioLocation.ResourceFile) {
                            AudioResourceOptimizer.AddTargetForClip(childVariation.resourceFileName, source);
                        }
                    } else {
                        sources.Add(new AudioInfo(variation, source, source.volume));
                        allVars.Add(variation);

                        if (variation.audLocation == MasterAudio.AudioLocation.ResourceFile) {
                            var resFileName = AudioResourceOptimizer.GetLocalizedFileName(variation.useLocalization, variation.resourceFileName);
                            AudioResourceOptimizer.AddTargetForClip(resFileName, source);
                        }
                    }
                }
            }

            // attach extra children from weight property.
            for (var i = 0; i < newWeightedChildren.Count; i++) {
                newWeightedChildren[i].parent = parentGroup;
            }

            _group = new AudioGroupInfo(sources, groupScript);
            if (groupScript.isSoloed) {
                soloedGroups.Add(groupScript);
            }

            if (audioSourcesBySoundType.ContainsKey(soundType)) {
                Debug.LogError("You have more than one SoundGroup named '" + soundType + "'. Ignoring the 2nd one. Please rename it.");
                continue;
            }

            // added code for persistent group volume
            var persistentVolume = PersistentAudioSettings.GetGroupVolume(soundType);
            if (persistentVolume.HasValue) {
                _group._group.groupMasterVolume = persistentVolume.Value;
            }

            audioSourcesBySoundType.Add(soundType, _group);

            for (var i = 0; i < sources.Count; i++) {
                playedStatuses.Add(i);
            }

            randomizer.Add(soundType, playedStatuses);
			randomizerLeftovers.Add(soundType, new List<int>(playedStatuses.Count));
			clipsPlayedBySoundTypeOldestFirst.Add(soundType, new List<int>());

            playedStatuses = new List<int>();
        }

        busFades.Clear();
        groupFades.Clear();

        Playlist pl = null;
        float firstSongLength = 0f;

        // check Syncrhonized Playlists for problems!
        for (var i = 0; i < musicPlaylists.Count; i++) {
            pl = musicPlaylists[i];
            if (pl.songTransitionType != SongFadeInPosition.SynchronizeClips) {
                continue;
            }

            if (pl.MusicSettings.Count < 2) {
                continue;
            }

            var songOne = pl.MusicSettings[0].clip;
            if (songOne == null) {
                continue;
            }

            firstSongLength = songOne.length;
            for (var s = 1; s < pl.MusicSettings.Count; s++) {
                var clip = pl.MusicSettings[s].clip;
                if (clip == null) {
                    continue;
                }
                 
                if (clip.length != firstSongLength) {
                    Debug.LogError("Playlist '" + pl.playlistName + "' is marked as Synchronized but the clip lengths are not all the same within the Playlist. Clips may not synchronize as intended.");
                    break;
                }
            }
        }

        // initialize persistent bus volumes 
        for (var i = 0; i < groupBuses.Count; i++) {
            var busName = groupBuses[i].busName;
            var busVol = PersistentAudioSettings.GetBusVolume(busName);

            if (!busVol.HasValue) {
                continue;
            }

            SetBusVolumeByName(busName, busVol.Value);
        }

        // populate ducking sounds dictionary
        duckingBySoundType.Clear();
        for (var i = 0; i < musicDuckingSounds.Count; i++) {
            var aDuck = musicDuckingSounds[i];
            duckingBySoundType.Add(aDuck.soundType, aDuck);
        }

        soundsLoaded = true;

        warming = true;

        // pre-warm the code so the first sound played for real doesn't have to JIT and be slow.
        if (!string.IsNullOrEmpty(firstGroupName)) {
            var result = PlaySound3DFollowTransform(firstGroupName, Trans, 0f, 1f, 0f, null);
            if (result != null && result.SoundPlayed) {
                result.ActingVariation.Stop();
            }
        }

		FireCustomEvent("FakeEvent", this.trans.position);

		// Event Sounds warmer
		var evts = GameObject.FindObjectsOfType(typeof(EventSounds));
		if (evts.Length > 0) {
			var evt = evts[0] as EventSounds;
			evt.PlaySounds(evt.particleCollisionSound, EventSounds.EventType.UserDefinedEvent);
		}

		// done warming
        warming = false;

        for (var i = 0; i < allVars.Count; i++) {
            allVars[i].DisableUpdater();
        }

		// fixed: make sure this happens before Playlists start or the volume won't be right.
		PersistentAudioSettings.RestoreMasterSettings();
    }

    void Start() {
        // wait for Playlist Controller to initialize!
        if (musicPlaylists.Count > 0
            && musicPlaylists[0].MusicSettings != null
            && musicPlaylists[0].MusicSettings.Count > 0
            && musicPlaylists[0].MusicSettings[0].clip != null
            && playlistControllersByName.Count == 0) {

            Debug.Log("No Playlist Controllers exist in the Scene. Music will not play.");
        }

        StartCoroutine(this.CoUpdate());
    }

    IEnumerator CoUpdate() {
        while (true) {
            this.frames++;

            if (MasterAudio.IgnoreTimeScale) {
                yield return StartCoroutine(CoroutineHelper.WaitForActualSeconds(INNER_LOOP_CHECK_INTERVAL));
            } else {
                yield return InnerLoopDelay;
            }

            // adjust for Inspector realtime slider.
            PerformBusFades();
            PerformGroupFades();
            RefillInactiveGroupPools();
        }
    }

    private static void UpdateRefillTime(string sType, float inactivePeriodSeconds) {
        if (!lastTimeSoundGroupPlayed.ContainsKey(sType)) {
            lastTimeSoundGroupPlayed.Add(sType, new SoundGroupRefillInfo(Time.realtimeSinceStartup, inactivePeriodSeconds));
        } else {
            lastTimeSoundGroupPlayed[sType]._lastTimePlayed = Time.realtimeSinceStartup;
        }
    }

    private void RefillInactiveGroupPools() {
        var groups = lastTimeSoundGroupPlayed.GetEnumerator();

        groupsToRemove.Clear();

        while (groups.MoveNext()) {
            var grp = groups.Current;
            if (grp.Value._lastTimePlayed + grp.Value._inactivePeriodSeconds < Time.realtimeSinceStartup) {
                RefillSoundGroupPool(grp.Key);
                groupsToRemove.Add(grp.Key);
            }
        }

        for (var i = 0; i < groupsToRemove.Count; i++) {
            lastTimeSoundGroupPlayed.Remove(groupsToRemove[i]);
        }
    }

    private void PerformBusFades() {
        BusFadeInfo aFader = null;
        GroupBus aBus = null;

        for (var i = 0; i < busFades.Count; i++) {
            aFader = busFades[i];
            if (!aFader.IsActive) {
                continue;
            }

            aBus = aFader.ActingBus;
            if (aBus == null) {
                Debug.Log("Could not find bus named '" + aFader.NameOfBus + "' to fade it one step.");
                aFader.IsActive = false;
                continue;
            }

            var newVolume = aBus.volume + aFader.VolumeStep;

            if (aFader.VolumeStep > 0f) {
                newVolume = Math.Min(newVolume, aFader.TargetVolume);
            } else {
                newVolume = Math.Max(newVolume, aFader.TargetVolume);
            }

            SetBusVolumeByName(aBus.busName, newVolume);

            if (newVolume == aFader.TargetVolume) {
                aFader.IsActive = false;

				if (stopZeroVolumeBuses && aFader.TargetVolume == 0f) {
					StopBus(aFader.NameOfBus);
				}

                if (aFader.completionAction != null) {
                    aFader.completionAction();
                }
            }
        }

        busFades.RemoveAll(delegate(BusFadeInfo obj) {
            return obj.IsActive == false;
        });
    }

    private void PerformGroupFades() {
        GroupFadeInfo aFader = null;
        MasterAudioGroup aGroup = null;
        for (var i = 0; i < groupFades.Count; i++) {
            aFader = groupFades[i];
            if (!aFader.IsActive) {
                continue;
            }

            aGroup = aFader.ActingGroup;
            if (aGroup == null) {
                Debug.Log("Could not find Sound Group named '" + aFader.NameOfGroup + "' to fade it one step.");
                aFader.IsActive = false;
                continue;
            }

            var newVolume = aGroup.groupMasterVolume + aFader.VolumeStep;

            if (aFader.VolumeStep > 0f) {
                newVolume = Math.Min(newVolume, aFader.TargetVolume);
            } else {
                newVolume = Math.Max(newVolume, aFader.TargetVolume);
            }

            SetGroupVolume(aGroup.GameObjectName, newVolume);

            if (newVolume == aFader.TargetVolume) {
                aFader.IsActive = false;

				if (stopZeroVolumeGroups && aFader.TargetVolume == 0f) {
					StopAllOfSound(aFader.NameOfGroup);
				}

				if (aFader.completionAction != null) {
                    aFader.completionAction();
                }
            }
        }

        groupFades.RemoveAll(delegate(GroupFadeInfo obj) {
            return obj.IsActive == false;
        });
    }

    void OnApplicationQuit() {
        AppIsShuttingDown = true; // very important!! Dont' take this out, false debug info may show up when you stop the Player
    }

    #endregion

    #region Sound Playing / Stopping Methods
    /// <summary>
    /// This method allows you to play a sound in a Sound Group in the location of the Master Audio prefab. Returns nothing.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    public static void PlaySoundAndForget(string sType, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null) {
        if (!SceneHasMasterAudio) {
            return;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            return;
        }

        PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, pitch, null, variationName, false, delaySoundTime, false, false);
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group in the location of the Master Audio prefab. Returns a PlaySoundResult object.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    /// <param name="isChaining"><b>Don't ever specify this</b> - used to control number of loops for Chained Loop Groups. MasterAudio will pass this parameter when it needs it. Never specify this param.</param>
    /// <param name="isSingleSubscribedPlay"><b>Don't ever specify this</b> - MasterAudio will pass this parameter when it needs it. Never specify this param.</param>
    /// <returns>PlaySoundResult - this object can be used to read if the sound played or not and also gives access to the Variation object that was used.</returns>
    public static PlaySoundResult PlaySound(string sType, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null, bool isChaining = false, bool isSingleSubscribedPlay = false) {
        if (!SceneHasMasterAudio) {
            return null;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            return null;
        }

        return PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, pitch, null, variationName, false, delaySoundTime, false, true, isChaining, isSingleSubscribedPlay);
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from a specific Vector 3 position. Returns nothing.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="sourcePosition">The position you want the sound to eminate from. Required.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    public static void PlaySound3DAtVector3AndForget(string sType, Vector3 sourcePosition, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null) {
        if (!SceneHasMasterAudio) {
            return;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            return;
        }

        PlaySoundAtVolume(sType, volumePercentage, sourcePosition, pitch, null, variationName, false, delaySoundTime, true, false);
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from a specific Vector3 position. Returns a PlaySoundResult object.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="sourcePosition">The position you want the sound to eminate from. Required.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    /// <returns>PlaySoundResult - this object can be used to read if the sound played or not and also gives access to the Variation object that was used.</returns>
    public static PlaySoundResult PlaySound3DAtVector3(string sType, Vector3 sourcePosition, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null) {
        if (!SceneHasMasterAudio) {
            return null;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            return null;
        }

        return PlaySoundAtVolume(sType, volumePercentage, sourcePosition, pitch, null, variationName, false, delaySoundTime, true, true);
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from a specific position - the position of a Transform you pass in. Returns nothing.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="sourceTrans">The Transform whose position you want the sound to eminate from. Pass null if you want to play the sound 2D.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    public static void PlaySound3DAtTransformAndForget(string sType, Transform sourceTrans = null, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null) {
        if (!SceneHasMasterAudio) {
            return;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            return;
        }

        PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, pitch, sourceTrans, variationName, false, delaySoundTime, false, false);
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from a specific position - the position of a Transform you pass in.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="sourceTrans">The Transform whose position you want the sound to eminate from. Pass null if you want to play the sound 2D.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    /// <param name="isChaining"><b>Don't ever specify this</b> - used to control number of loops for Chained Loop Groups. MasterAudio will pass this parameter when it needs it. Never specify this param.</param>
    /// <param name="isSingleSubscribedPlay"><b>Don't ever specify this</b> - MasterAudio will pass this parameter when it needs it. Never specify this param.</param>
    /// <returns>PlaySoundResult - this object can be used to read if the sound played or not and also gives access to the Variation object that was used.</returns>
    public static PlaySoundResult PlaySound3DAtTransform(string sType, Transform sourceTrans = null, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null, bool isChaining = false, bool isSingleSubscribedPlay = false) {
        if (!SceneHasMasterAudio) {
            return null;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            return null;
        }

        return PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, pitch, sourceTrans, variationName, false, delaySoundTime, false, true, isChaining, isSingleSubscribedPlay);
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from a specific position - a Transform you pass in. Returns nothing.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="sourceTrans">The Transform whose position you want the sound to eminate from. Pass null if you want to play the sound 2D.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    public static void PlaySound3DFollowTransformAndForget(string sType, Transform sourceTrans = null, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null) {
        if (!SceneHasMasterAudio) {
            return;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            return;
        }

        PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, pitch, sourceTrans, variationName, true, delaySoundTime, false, false);
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from a specific position - a Transform you pass in, and it will follow the Transform if it moves. Returns a PlaySoundResult.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="sourceTrans">The Transform whose position you want the sound to eminate from. Pass null if you want to play the sound 2D.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    /// <param name="isChaining"><b>Don't ever specify this</b> - used to control number of loops for Chained Loop Groups. MasterAudio will pass this parameter when it needs it. Never specify this param.</param>
    /// <param name="isSingleSubscribedPlay"><b>Don't ever specify this</b> - MasterAudio will pass this parameter when it needs it. Never specify this param.</param>
    /// <returns>PlaySoundResult - this object can be used to read if the sound played or not and also gives access to the Variation object that was used.</returns>
    public static PlaySoundResult PlaySound3DFollowTransform(string sType, Transform sourceTrans = null, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null, bool isChaining = false, bool isSingleSubscribedPlay = false) {
        if (!SceneHasMasterAudio) {
            return null;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            return null;
        }

        return PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, pitch, sourceTrans, variationName, true, delaySoundTime, false, true, isChaining, isSingleSubscribedPlay);
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from a specific position - a Transform you pass in. Returns nothing.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="sourceTrans">The Transform whose position you want the sound to eminate from. Pass null if you want to play the sound 2D.</param>
    /// <param name="attachToSource"><b>Optional</b> - defaults to False. If you specify true, and also passed a non-null value for sourceTrans, the Sound Variation will be attached to the sourceTrans object so that the sound can follow it.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    public static void PlaySound3DAndForget(string sType, Transform sourceTrans = null, bool attachToSource = false, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null) {
        if (!SceneHasMasterAudio) {
            return;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            return;
        }

        PlaySoundAtVolume(sType, volumePercentage, Vector3.zero, pitch, sourceTrans, variationName, attachToSource, delaySoundTime, false, false);
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from the location of Master Audio. This method will not return until the sound is finished (or cannot play) to continue execution. You need to call this with StartCoroutine. The sound will not be played looped, since that could cause a Coroutine that would never end.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    public static IEnumerator PlaySoundAndWaitUntilFinished(string sType, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null) {
        if (!SceneHasMasterAudio) {
            yield break;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            yield break;
        }

        var sound = PlaySound(sType, volumePercentage, pitch, delaySoundTime, variationName, false, true);
        var done = false;

        if (sound == null || sound.ActingVariation == null) {
            yield break; // sound was "busy" or couldn't play for some reason.
        }
        sound.ActingVariation.SoundFinished += delegate {
            done = true;
        };

        while (!done) {
            yield return MasterAudio.endOfFrameDelay;
        }
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from a specific position - a Transform you pass in. This method will not return until the sound is finished (or cannot play) to continue execution. You need to call this with StartCoroutine. The sound will not be played looped, since that could cause a Coroutine that would never end.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="sourceTrans">The Transform whose position you want the sound to eminate from. Pass null if you want to play the sound 2D.</param>
    /// <param name="attachToSource"><b>Optional</b> - defaults to False. If you specify true, and also passed a non-null value for sourceTrans, the Sound Variation will be attached to the sourceTrans object so that the sound can follow it.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    public static IEnumerator PlaySound3DAtTransformAndWaitUntilFinished(string sType, Transform sourceTrans, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null) {
        if (!SceneHasMasterAudio) {
            yield break;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            yield break;
        }

        var sound = PlaySound3DAtTransform(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName, false, true);
        var done = false;

        if (sound == null || sound.ActingVariation == null) {
            yield break; // sound was "busy" or couldn't play for some reason.
        }
        sound.ActingVariation.SoundFinished += delegate {
            done = true;
        };

        while (!done) {
            yield return MasterAudio.endOfFrameDelay;
        }
    }

    /// <summary>
    /// This method allows you to play a sound in a Sound Group from a specific position - a Transform you pass in, and it will follow the Transform if it moves. This method will not return until the sound is finished (or cannot play) to continue execution. You need to call this with StartCoroutine. The sound will not be played looped, since that could cause a Coroutine that would never end.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to trigger a sound from.</param>
    /// <param name="sourceTrans">The Transform whose position you want the sound to eminate from. Pass null if you want to play the sound 2D.</param>
    /// <param name="attachToSource"><b>Optional</b> - defaults to False. If you specify true, and also passed a non-null value for sourceTrans, the Sound Variation will be attached to the sourceTrans object so that the sound can follow it.</param>
    /// <param name="volumePercentage"><b>Optional</b> - used if you want to play the sound at a reduced volume (between 0 and 1).</param>
    /// <param name="pitch"><b>Optional</b> - used if you want to play the sound at a specific pitch. If you do, it will override the pich and random pitch in the variation.</param>
    /// <param name="delaySoundTime"><b>Optional</b> - used if you want to play the sound X seconds from now instead of immediately.</param>
    /// <param name="variationName"><b>Optional</b> - used if you want to play a specific variation by name. Otherwise a random variation is played.</param>
    public static IEnumerator PlaySound3DFollowTransformAndWaitUntilFinished(string sType, Transform sourceTrans, float volumePercentage = 1f, float? pitch = null, float delaySoundTime = 0f, string variationName = null) {
        if (!SceneHasMasterAudio) {
            yield break;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot play: " + sType);
            yield break;
        }

        var sound = PlaySound3DFollowTransform(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName, false, true);
        var done = false;

        if (sound == null || sound.ActingVariation == null) {
            yield break; // sound was "busy" or couldn't play for some reason.
        }
        sound.ActingVariation.SoundFinished += delegate {
            done = true;
        };

        while (!done) {
            yield return MasterAudio.endOfFrameDelay;
        }
    }

    private static PlaySoundResult PlaySoundAtVolume(string sType,
            float volumePercentage,
            Vector3 sourcePosition,
            float? pitch = null,
            Transform sourceTrans = null,
            string variationName = null,
            bool attachToSource = false,
            float delaySoundTime = 0f,
            bool useVector3 = false,
            bool makePlaySoundResult = false,
            bool isChaining = false,
            bool isSingleSubscribedPlay = false, 
            bool triggeredAsChildGroup = false) {

        if (!SceneHasMasterAudio) { // No MA
            return null;
        }

        if (!SoundsReady || sType == string.Empty || sType == NO_GROUP_NAME) {
            return null; // not awake yet
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            var msg = "MasterAudio could not find sound: " + sType + ". If your Scene just changed, this could happen when an OnDisable or OnInvisible event sound happened to a per-scene sound, which is expected.";
            if (sourceTrans != null) {
                msg += " Triggered by prefab: " + (sourceTrans == null ? "Unknown" : sourceTrans.name);
            }

            LogWarning(msg);
            return null;
        }

        AudioGroupInfo _group = audioSourcesBySoundType[sType];
        MasterAudioGroup maGroup = _group._group;

		var loggingEnabledForGrp = LoggingEnabledForGroup(maGroup);

		if (_group._group.childGroupMode == MasterAudioGroup.ChildGroupMode.TriggerLinkedGroupsWhenRequested && !triggeredAsChildGroup) {
            for (var i = 0; i < _group._group.childSoundGroups.Count; i++) {
                var childGrpName = _group._group.childSoundGroups[i];
                PlaySoundAtVolume(childGrpName, volumePercentage, sourcePosition, pitch, sourceTrans, null, attachToSource, delaySoundTime, useVector3, false, false, false, true);
            }
        }

        if (MasterAudio.Instance.mixerMuted) {
            if (loggingEnabledForGrp) {
				LogMessage("MasterAudio skipped playing sound: " + sType + " because the Mixer is muted.");
			}
            return null;
        }
        if (maGroup.isMuted) {
			if (loggingEnabledForGrp) {
				LogMessage("MasterAudio skipped playing sound: " + sType + " because the Group is muted.");
			}
            return null;
        }
        if (soloedGroups.Count > 0 && !soloedGroups.Contains(maGroup)) {
			if (loggingEnabledForGrp) {
				LogMessage("MasterAudio skipped playing sound: " + sType + " because there are one or more Groups soloed. This one is not.");
			}
            return null;
        }

        if (maGroup.curVariationMode == MasterAudioGroup.VariationMode.Normal) {
            switch (maGroup.limitMode) {
                case MasterAudioGroup.LimitMode.TimeBased:
                    if (maGroup.minimumTimeBetween > 0) {
                        if (Time.realtimeSinceStartup < (_group._lastTimePlayed + maGroup.minimumTimeBetween)) {
							if (loggingEnabledForGrp) {
								LogMessage("MasterAudio skipped playing sound: " + sType + " due to Group's Min Seconds setting.");
							}
                            return null;
                        }

                        _group._lastTimePlayed = Time.realtimeSinceStartup;
                    }
                    break;
                case MasterAudioGroup.LimitMode.FrameBased:
                    if (Time.frameCount - _group._lastFramePlayed < maGroup.limitPerXFrames) {
						if (loggingEnabledForGrp) {	
							LogMessage("Master Audio skipped playing sound: " + sType + " due to Group's Per Frame Limit.");
						}
                        return null;
                    }

                    _group._lastFramePlayed = Time.frameCount;

                    break;
                case MasterAudioGroup.LimitMode.None:
                    _group._lastTimePlayed = Time.realtimeSinceStartup;
                    _group._lastFramePlayed = Time.frameCount;
                    break;
            }
        }

        var sources = _group._sources;
        var isNonSpecific = string.IsNullOrEmpty(variationName);

        if (sources.Count == 0) {
            Debug.Log("Sound Group {" + sType + "} has no active variations.");
            return null;
        }

        if (maGroup.curVariationMode == MasterAudioGroup.VariationMode.Normal) {
            if (_group._group.limitPolyphony) {
                var maxVoices = _group._group.voiceLimitCount;
                var busyVoices = 0;
                for (var i = 0; i < _group._sources.Count; i++) {
                    if (_group._sources[i].source == null) {
                        continue;
                    }
                    if (!_group._sources[i].source.isPlaying) {
                        continue;
                    }

                    busyVoices++;
                    if (busyVoices >= maxVoices) {
						if (loggingEnabledForGrp) {
							LogMessage("Polyphony limit of group: " + _group._group.GameObjectName + " exceeded. Will not play this sound for this instance.");
						}
                        return null;
                    }
                }
            }
        }

        var groupBus = _group._group.BusForGroup;
        if (groupBus != null) {
            if (groupBus.BusVoiceLimitReached) {
				if (loggingEnabledForGrp) {
					LogMessage("Bus voice limit has been reached. Cannot play the sound: " + _group._group.GameObjectName + " until one voice has stopped playing.");
				}
                return null;
            } else if (groupBus.ShouldStopAllBeforePlaying) {
                StopBus(groupBus.busName);
            }
        }

        AudioInfo randomSource = null;

        if (sources.Count == 1) {
			if (loggingEnabledForGrp) {
				LogMessage("Cueing only child of " + sType);
			}
            randomSource = sources[0];
        }

        List<int> choices = null;
        int? randomIndex = null;
		List<int> otherChoices = null;

        if (_group._group.curVariationMode == MasterAudioGroup.VariationMode.Dialog) {
            if (_group._group.useDialogFadeOut) {
                FadeOutAllOfSound(_group._group.GameObjectName, _group._group.dialogFadeOutTime);
            } else {
                StopAllOfSound(_group._group.GameObjectName);
            }
        }

        var pickedChoice = -1;

        if (randomSource == null) { // we must get a non-busy random source!
            if (!randomizer.ContainsKey(sType)) {
                Debug.Log("Sound Group {" + sType + "} has no active variations.");
                return null;
            }

            if (isNonSpecific) {
                choices = randomizer[sType];

                if (_group._group.curVariationSequence == MasterAudioGroup.VariationSequence.Randomized) {
                    randomIndex = UnityEngine.Random.Range(0, choices.Count);
                } else {
                    // top to bottom! 
                    randomIndex = 0;
                }

                pickedChoice = choices[randomIndex.Value];
                randomSource = sources[pickedChoice];

                // fill list with other random sources not used yet in case the first is busy.
				otherChoices = randomizerLeftovers[sType];
                otherChoices.Remove(pickedChoice);

				if (loggingEnabledForGrp) {
                	LogMessage(string.Format("Cueing child {0} of {1}",
   	                	choices[randomIndex.Value],
                    	sType));
				}
            } else {
                // find source by name
                var isFound = false;
                var matchesFound = 0;
                for (var i = 0; i < sources.Count; i++) {
                    var aSource = sources[i];
                    if (aSource.source.name != variationName) {
                        continue;
                    }

                    matchesFound++;
                    if (!aSource.variation.IsAvailableToPlay) {
                        continue;
                    }

                    randomSource = aSource;
                    isFound = true;
                    pickedChoice = i;
                    break;
                }

                if (!isFound) {
                    if (matchesFound == 0) {
						if (loggingEnabledForGrp) {
							LogMessage("Can't find variation {" + variationName + "} of " + sType);
						}
                    } else {
						if (loggingEnabledForGrp) {
							LogMessage("Can't find non-busy variation {" + variationName + "} of " + sType);
						}
                    }

                    return null;
                }

				if (loggingEnabledForGrp) {
					LogMessage(string.Format("Cueing child named '{0}' of {1}",
                    variationName,
                    sType));
				}
            }
        }

        PlaySoundResult playedState = null;
        bool playedSound = false;
        bool forgetSoundPlayedOrScheduled = false;

        var soundSuccess = false;
        bool makePSRsuccess = false;
        bool doNotMakePSRsuccess = false;

        do {
            playedState = PlaySoundIfAvailable(sType, randomSource, sourcePosition, volumePercentage, ref forgetSoundPlayedOrScheduled, pitch, _group, sourceTrans, attachToSource, delaySoundTime, useVector3, makePlaySoundResult, isChaining, isSingleSubscribedPlay);

            makePSRsuccess = makePlaySoundResult && (playedState != null && (playedState.SoundPlayed || playedState.SoundScheduled));
            doNotMakePSRsuccess = !makePlaySoundResult && forgetSoundPlayedOrScheduled;

            soundSuccess = makePSRsuccess || doNotMakePSRsuccess;

            //Debug.Log(soundSuccess);

            if (soundSuccess) {
                playedSound = true;

                if (isNonSpecific && randomIndex.HasValue) { // only if successfully played!
                    choices.RemoveAt(randomIndex.Value);
                    clipsPlayedBySoundTypeOldestFirst[sType].Add(pickedChoice);

                    if (choices.Count == 0) {
						if (loggingEnabledForGrp) {
							LogMessage("Refilling Variation pool: " + sType);
						}

                        RefillSoundGroupPool(sType);
                    }
                }

                if (_group._group.curVariationSequence == MasterAudioGroup.VariationSequence.TopToBottom && _group._group.useInactivePeriodPoolRefill) {
                    UpdateRefillTime(sType, _group._group.inactivePeriodSeconds);
                }
            } else if (isNonSpecific) {
                // try the other ones
                if (otherChoices != null && otherChoices.Count > 0) {
                    randomSource = sources[otherChoices[0]];
					if (loggingEnabledForGrp) {
						LogMessage("Child was busy. Cueing child {" + sources[otherChoices[0]].source.name + "} of " + sType);
					}
                    otherChoices.RemoveAt(0);
                }
            } else {
				if (loggingEnabledForGrp) {
					LogMessage("Child was busy. Since you wanted a named Variation, no others to try. Aborting.");
				}
                if (otherChoices != null) {
					otherChoices.Clear();
				}
            }
        }
        while (!playedSound && otherChoices != null && otherChoices.Count > 0); // repeat until you've either played the sound or exhausted all possibilities.

        if (!soundSuccess) {
			if (loggingEnabledForGrp) {
				LogMessage("All children of " + sType + " were busy. Will not play this sound for this instance.");
			}
        } else {
			if (_group._group.childGroupMode == MasterAudioGroup.ChildGroupMode.TriggerLinkedGroupsWhenPlayed && !triggeredAsChildGroup) {
				for (var i = 0; i < _group._group.childSoundGroups.Count; i++) {
					var childGrpName = _group._group.childSoundGroups[i];
					PlaySoundAtVolume(childGrpName, volumePercentage, sourcePosition, pitch, sourceTrans, null, attachToSource, delaySoundTime, useVector3, false, false, false, true);
				}
			}
		}

        return playedState;
    }

    private static PlaySoundResult PlaySoundIfAvailable(string soundGroupName,
            AudioInfo info,
            Vector3 sourcePosition,
            float volumePercentage,
            ref bool forgetSoundPlayed,
            float? pitch = null,
            AudioGroupInfo audioGroup = null,
            Transform sourceTrans = null,
            bool attachToSource = false,
            float delaySoundTime = 0f,
            bool useVector3 = false,
            bool makePlaySoundResult = false,
            bool isChaining = false,
            bool isSingleSubscribedPlay = false) {

        if (info.source == null) {
            // this avoids false errors when stopping the game (from became "invisible" event callers)
            return null;
        }

        MasterAudioGroup maGroup = audioGroup._group;

        if (maGroup.curVariationMode == MasterAudioGroup.VariationMode.Normal && info.source.isPlaying) {
            var playedPercentage = AudioUtil.GetAudioPlayedPercentage(info.source);
            var retriggerPercent = maGroup.retriggerPercentage;

            if (playedPercentage < retriggerPercent) {
                return null; // wait for this to stop playing or play further.
            }
        }

        info.variation.Stop();
        info.variation.ObjectToFollow = null;

        bool shouldUseClipAgePriority = Instance.prioritizeOnDistance && (Instance.useClipAgePriority || info.variation.ParentGroup.useClipAgePriority);
		
		if (useVector3) {
            info.source.transform.position = sourcePosition;
            if (MasterAudio.Instance.prioritizeOnDistance) {
                AudioPrioritizer.Set3dPriority(info.source, shouldUseClipAgePriority);
            }
        } else if (sourceTrans != null) {
            if (attachToSource) {
                info.variation.ObjectToFollow = sourceTrans;
            } else {
                info.source.transform.position = sourceTrans.position;
                info.variation.ObjectToTriggerFrom = sourceTrans;
            }

            if (MasterAudio.Instance.prioritizeOnDistance) {
                AudioPrioritizer.Set3dPriority(info.source, shouldUseClipAgePriority);
            }
        } else {
            // "2d manner" - from Master Audio location
            if (MasterAudio.Instance.prioritizeOnDistance) {
                AudioPrioritizer.Set2dSoundPriority(info.source);
            }
            info.source.transform.localPosition = Vector3.zero; // put it back in MA prefab position after being detached.
        }

        var groupVolume = maGroup.groupMasterVolume;
        var busVolume = GetBusVolume(maGroup);

		var varVol = info.originalVolume;

		var randomVol = 0f;
		if (info.variation.useRandomVolume) {
			// random volume
			randomVol = UnityEngine.Random.Range(info.variation.randomVolumeMin, info.variation.randomVolumeMax);
			
			switch (info.variation.randomVolumeMode) {
			case SoundGroupVariation.RandomVolumeMode.AddToClipVolume:
				varVol += randomVol;
				break;
			case SoundGroupVariation.RandomVolumeMode.IgnoreClipVolume:
				varVol = randomVol;
				break;
			}
		}

		float calcVolume = varVol * groupVolume * busVolume * MasterAudio.Instance.masterAudioVolume;

        // set volume to percentage.
        float volume = calcVolume * volumePercentage;
        var targetVolume = volume;

        info.source.volume = targetVolume;

        // save these for on the fly adjustments afterward
        info.lastPercentageVolume = volumePercentage;
        info.lastRandomVolume = randomVol;

        bool isActive = false;

#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
        isActive =  info.variation.GameObj.active;
#else
		isActive = info.variation.GameObj.activeInHierarchy;
#endif

        if (!isActive) {
            return null;
        }

        PlaySoundResult result = null;

        if (makePlaySoundResult) {
            result = new PlaySoundResult();
            result.ActingVariation = info.variation;

            if (delaySoundTime > 0f) {
                result.SoundScheduled = true;
            } else {
                result.SoundPlayed = true;
            }
        } else {
            forgetSoundPlayed = true;
        }

		var soundType = maGroup.GameObjectName;
		var isChainLoop = maGroup.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain;

        if (isChainLoop) {
		    if (!isChaining) {
            	maGroup.ChainLoopCount = 0;
			}
		
			// make sure there isn't 2 going, ever
			MasterAudio.StopAllOfSound(soundType);
		}

		info.variation.Play(pitch, targetVolume, soundType, volumePercentage, targetVolume, pitch, sourceTrans, attachToSource, delaySoundTime, isChaining, isSingleSubscribedPlay);

        return result;
    }

    public static void DuckSoundGroup(string soundGroupName, AudioSource aSource) {
        var ma = MasterAudio.Instance;

        if (!ma.EnableMusicDucking || !ma.duckingBySoundType.ContainsKey(soundGroupName)) {
            return;
        }

        var matchingDuck = ma.duckingBySoundType[soundGroupName];

        // duck music
        var duckLength = aSource.clip.length;
        var duckPitch = aSource.pitch;

        var pcs = PlaylistController.Instances;
        for (var i = 0; i < pcs.Count; i++) {
            pcs[i].DuckMusicForTime(duckLength, duckPitch, matchingDuck.riseVolStart);
        }

        if (pcs.Count == 0) {
            Debug.LogWarning("Playlist Controller is not in the Scene. Cannot duck music.");
        }
    }

    private static void StopPauseOrUnpauseSoundsOfTransform(Transform trans, List<AudioInfo> varList, VariationCommand varCmd) {
        MasterAudioGroup grp = null;

        for (var v = 0; v < varList.Count; v++) {
            var variation = varList[v].variation;
            if (!variation.WasTriggeredFromTransform(trans)) {
                continue;
            }

            if (grp == null) {
                var sType = variation.ParentGroup.GameObjectName;
                grp = GrabGroup(sType);
            }

            var stopEndDetector = grp != null && grp.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain;

            // matched, stop or pause the audio.
            switch (varCmd) {
                case VariationCommand.Stop:
                    variation.Stop(stopEndDetector);
                    break;
                case VariationCommand.Pause:
                    variation.Pause();
                    break;
                case VariationCommand.Unpause:
                    if (AudioUtil.IsAudioPaused(variation.VarAudio)) {
                        variation.VarAudio.Play();
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// This method allows you to abruptly stop all sounds triggered by or following a Transform.
    /// </summary>
    /// <param name="trans">The Transform the sound was triggered to follow or use the position of.</param>
    public static void StopAllSoundsOfTransform(Transform trans) {
        if (!SceneHasMasterAudio) { // No MA
            return;
        }

        foreach (var key in audioSourcesBySoundType.Keys) {
            var varList = audioSourcesBySoundType[key]._sources;
            StopPauseOrUnpauseSoundsOfTransform(trans, varList, VariationCommand.Stop);
        }
    }

    /// <summary>
    /// This method allows you to abruptly stop all sounds of a particular Sound Group triggered by or following a Transform.
    /// </summary>
    /// <param name="trans">The Transform the sound was triggered to follow or use the position of.</param>
    /// <param name="sType">The name of the Sound Group to stop.</param>
    public static void StopSoundGroupOfTransform(Transform trans, string sType) {
        if (!SceneHasMasterAudio) { // No MA
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var varList = audioSourcesBySoundType[sType]._sources;
        StopPauseOrUnpauseSoundsOfTransform(trans, varList, VariationCommand.Stop);
    }

    /// <summary>
    /// This method allows you to pause all sounds triggered by or following a Transform.
    /// </summary>
    /// <param name="trans">The Transform the sound was triggered to follow or use the position of.</param>
    public static void PauseAllSoundsOfTransform(Transform trans) {
        if (!SceneHasMasterAudio) { // No MA
            return;
        }

        foreach (var key in audioSourcesBySoundType.Keys) {
            var varList = audioSourcesBySoundType[key]._sources;
            StopPauseOrUnpauseSoundsOfTransform(trans, varList, VariationCommand.Pause);
        }
    }

    /// <summary>
    /// This method allows you to pause all sounds of a particular Sound Group triggered by or following a Transform.
    /// </summary>
    /// <param name="trans">The Transform the sound was triggered to follow or use the position of.</param>
    /// <param name="sType">The name of the Sound Group to stop.</param>
    public static void PauseSoundGroupOfTransform(Transform trans, string sType) {
        if (!SceneHasMasterAudio) { // No MA
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var varList = audioSourcesBySoundType[sType]._sources;
        StopPauseOrUnpauseSoundsOfTransform(trans, varList, VariationCommand.Pause);
    }

    /// <summary>
    /// This method allows you to unpause all sounds triggered by or following a Transform.
    /// </summary>
    /// <param name="trans">The Transform the sound was triggered to follow or use the position of.</param>
    public static void UnpauseAllSoundsOfTransform(Transform trans) {
        if (!SceneHasMasterAudio) { // No MA
            return;
        }

        foreach (var key in audioSourcesBySoundType.Keys) {
            var varList = audioSourcesBySoundType[key]._sources;
            StopPauseOrUnpauseSoundsOfTransform(trans, varList, VariationCommand.Unpause);
        }
    }

    /// <summary>
    /// This method allows you to unpause all sounds of a particular Sound Group triggered by or following a Transform.
    /// </summary>
    /// <param name="trans">The Transform the sound was triggered to follow or use the position of.</param>
    /// <param name="sType">The name of the Sound Group to stop.</param>
    public static void UnpauseSoundGroupOfTransform(Transform trans, string sType) {
        if (!SceneHasMasterAudio) { // No MA
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var varList = audioSourcesBySoundType[sType]._sources;
        StopPauseOrUnpauseSoundsOfTransform(trans, varList, VariationCommand.Unpause);
    }

    /// <summary>
    /// This method allows you to fade out all sounds of a particular Sound Group triggered by or following a Transform for X seconds.
    /// </summary>
    /// <param name="trans">The Transform the sound was triggered to follow or use the position of.</param>
    /// <param name="sType">The name of the Sound Group.</param>
    /// <param name="fadeTime">The amount of seconds the fading will take.</param>
    public static void FadeOutSoundGroupOfTransform(Transform trans, string sType, float fadeTime) {
        if (!SceneHasMasterAudio) { // No MA
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var varList = audioSourcesBySoundType[sType]._sources;
        for (var v = 0; v < varList.Count; v++) {
            var variation = varList[v].variation;
            if (!variation.WasTriggeredFromTransform(trans)) {
                continue;
            }
            variation.FadeOutNow(fadeTime);
        }
    }

    /// <summary>
    /// This method allows you to abruptly stop all sounds in a specified Sound Group.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    public static void StopAllOfSound(string sType) {
        if (!SceneHasMasterAudio) { // No MA
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var sources = audioSourcesBySoundType[sType]._sources;

        var _grp = GrabGroup(sType);

        var stopEndDetector = _grp != null && _grp.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain;

        foreach (AudioInfo audio in sources) {
            audio.variation.Stop(stopEndDetector);
        }
    }

    /// <summary>
    /// This method allows you to fade out all sounds in a specified Sound Group for X seconds.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    /// <param name="fadeTime">The amount of seconds the fading will take.</param>
    public static void FadeOutAllOfSound(string sType, float fadeTime) {
        if (!SceneHasMasterAudio) { // No MA
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var sources = audioSourcesBySoundType[sType]._sources;

        foreach (AudioInfo audio in sources) {
            audio.variation.FadeOutNow(fadeTime);
        }
    }
    #endregion

    #region Particle methods
    /// <summary>
    /// This method will trigger particles from a Transform you specify.
    /// </summary>
    /// <param name="trans">The Transform whose ParticleSystem you will be using.</param>
    /// <param name="particleCount">The number of particles to spawn.</param>
    public static void TriggerParticleEmission(Transform trans, int particleCount) {
        ParticleSystem part = trans.GetComponent<ParticleSystem>();
        if (part == null) {
            return;
        }

        part.Emit(particleCount);
    }
    #endregion

    #region Variation methods
    /// <summary>
    /// This method will add the variation to a Sound Group during runtime.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    /// <param name="clip">The Audio Clip of the variation.</param>
    /// <param name="variationName">Use this to specify a the variation's name.</param>
    /// <param name="volume">Use this to specify a the variation's volume.</param>
    /// <param name="pitch">Use this to specify a the variation's pitch.</param>
    public static void CreateGroupVariationFromClip(string sType, AudioClip clip, string variationName, float volume = 1f, float pitch = 1f) {
        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot create change variation clip yet.");
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var grp = audioSourcesBySoundType[sType];

        var matchingNameFound = false;

        for (var i = 0; i < grp._sources.Count; i++) {
            var aVar = grp._sources[i];
            if (aVar.variation.name == variationName) {
                matchingNameFound = true;
                break;
            }
        }

        if (matchingNameFound) {
            MasterAudio.LogWarning("You already have a Variation for this Group named '" + variationName + "'. \n\nPlease rename these Variations when finished to be unique, or you may not be able to play them by name if you have a need to.");
        }

        var newVar = (GameObject)GameObject.Instantiate(MasterAudio.Instance.soundGroupVariationTemplate.gameObject, grp._group.transform.position, Quaternion.identity);

        newVar.transform.name = variationName;
        newVar.transform.parent = grp._group.transform;
        
		var audSrc = newVar.GetComponent<AudioSource>();
		audSrc.clip = clip;
		audSrc.pitch = pitch;

        var newVariation = newVar.GetComponent<SoundGroupVariation>();
        if (newVariation.VariationUpdater != null) {
            newVariation.DisableUpdater();
        }

        var newInfo = new AudioInfo(newVariation, newVariation.VarAudio, volume);

        grp._sources.Add(newInfo);

        if (randomizer.ContainsKey(sType)) { // sanity check
            randomizer[sType].Add(grp._sources.Count - 1);
			randomizerLeftovers[sType].Add(grp._sources.Count - 1);
        }
    }


    /// <summary>
    /// This method will change the pitch of a variation or all variations in a Sound Group.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    /// <param name="changeAllVariations">Whether to change all variations in the Sound Group or just one.</param>
    /// <param name="variationName">Use this to specify a certain variation's name. Only that variation will be changes if you haven't passed changeAllVariations as true.</param>
    /// <param name="pitch">The new pitch of the variation.</param>
    public static void ChangeVariationPitch(string sType, bool changeAllVariations, string variationName, float pitch) {
        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot change variation clip yet.");
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var grp = audioSourcesBySoundType[sType];

        var iChanged = 0;

        for (var i = 0; i < grp._sources.Count; i++) {
            var aVar = grp._sources[i];
            if (changeAllVariations || aVar.source.transform.name == variationName) {
                aVar.variation.original_pitch = pitch;
                var aud = aVar.variation.VarAudio;
                if (aud != null) {
                    aud.pitch = pitch;
                }
                iChanged++;
            }
        }

        if (iChanged == 0 && !changeAllVariations) {
            Debug.Log("Could not find any matching variations of Sound Group '" + sType + "' to change the pitch of.");
        }
    }

    /// <summary>
    /// This method will change the volume of a variation or all variations in a Sound Group.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    /// <param name="changeAllVariations">Whether to change all variations in the Sound Group or just one.</param>
    /// <param name="variationName">Use this to specify a certain variation's name. Only that variation will be changes if you haven't passed changeAllVariations as true.</param>
    /// <param name="volume">The new volume of the variation.</param>
    public static void ChangeVariationVolume(string sType, bool changeAllVariations, string variationName, float volume) {
        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot change variation clip yet.");
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var grp = audioSourcesBySoundType[sType];

        var iChanged = 0;

        for (var i = 0; i < grp._sources.Count; i++) {
            var aVar = grp._sources[i];
            if (changeAllVariations || aVar.source.transform.name == variationName) {
                aVar.originalVolume = volume;
                iChanged++;
            }
        }

        if (iChanged == 0 && !changeAllVariations) {
            Debug.Log("Could not find any matching variations of Sound Group '" + sType + "' to change the volume of.");
        }
    }

    /// <summary>
    /// This method will change the Audio Clip used by a variation into one named from a Resource file.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    /// <param name="changeAllVariations">Whether to change all variations in the Sound Group or just one.</param>
    /// <param name="variationName">Use this to specify a certain variation's name. Only that variation will be changes if you haven't passed changeAllVariations as true.</param>
    /// <param name="resourceFileName">The name of the file in the Resource.</param>
    public static void ChangeVariationClipFromResources(string sType, bool changeAllVariations, string variationName, string resourceFileName) {
        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot create change variation clip yet.");
            return;
        }

        var aClip = Resources.Load(resourceFileName) as AudioClip;

        if (aClip == null) {
            LogWarning("Resource file '" + resourceFileName + "' could not be located.");
            return;
        }

        ChangeVariationClip(sType, changeAllVariations, variationName, aClip);
    }

    /// <summary>
    /// This method will change the Audio Clip used by a variation into one you specify.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    /// <param name="changeAllVariations">Whether to change all variations in the Sound Group or just one.</param>
    /// <param name="variationName">Use this to specify a certain variation's name. Only that variation will be changes if you haven't passed changeAllVariations as true.</param>
    /// <param name="clip">The Audio Clip to replace the old one with.</param>
    public static void ChangeVariationClip(string sType, bool changeAllVariations, string variationName, AudioClip clip) {
        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot create change variation clip yet.");
            return;
        }

        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            Debug.LogWarning("Could not locate group '" + sType + "'.");
            return;
        }

        var grp = audioSourcesBySoundType[sType];

        for (var i = 0; i < grp._sources.Count; i++) {
            var aVar = grp._sources[i];
            if (changeAllVariations || aVar.source.transform.name == variationName) {
                aVar.source.clip = clip;
            }
        }
    }

    #endregion

    #region Sound Group methods
    /// <summary>
    /// This method will return the length in seconds of a Variation in a Sound Group.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    /// <param name="variationName">Use this to specify a certain variation's name. The first match will be used</param>
    /// <returns>The time length of the Variation, taking pitch into account. If it cannot find the Variation, it returns -1 and logs the reason to the console.</returns>
    public static float GetVariationLength(string sType, string variationName) {
        var grp = GrabGroup(sType);
        if (grp == null) {
            return -1f;
        }

		SoundGroupVariation match = null;
		
		foreach (SoundGroupVariation sgv in grp.groupVariations) {
			if (sgv.name == variationName) {
		        match = sgv;
				break;
			}
		}

        if (match == null) {
            LogError("Could not find Variation '" + variationName + "' in Sound Group '" + sType + "'.");
            return -1f;
        }

        if (match.audLocation == AudioLocation.ResourceFile) {
            LogError("Variation '" + variationName + "' in Sound Group '" + sType + "' length cannot be determined because it's a Resource Files.");
            return -1f;
        }

        var clip = match.VarAudio.clip;
        if (clip == null) {
            LogError("Variation '" + variationName + "' in Sound Group '" + sType + "' has no Audio Clip.");
            return -1f;
        }

        if (match.VarAudio.pitch <= 0f) {
            LogError("Variation '" + variationName + "' in Sound Group '" + sType + "' has negative or zero pitch. Cannot compute length.");
            return -1f;
        }

        return clip.length / match.VarAudio.pitch;
    }

    /// <summary>
    /// This method allows you to refill the pool of the Variation sounds for a Sound Group. That way you don't have to wait for all remaining random (or top to bottom) sounds to be played before it refills.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to refill the pool of.</param>
    public static void RefillSoundGroupPool(string sType) {
        var grp = GrabGroup(sType, false);
        if (grp == null) {
            return;
        }

        var choices = randomizer[sType];

        var played = clipsPlayedBySoundTypeOldestFirst[sType];
        if (choices.Count > 0) {
            played.AddRange(choices); // add any not played yet.
            choices.Clear();
        }

        choices.AddRange(played);
        played.Clear();

        if (grp.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain) {
            grp.ChainLoopCount++;
        }
    }

    /// <summary>
    /// This method allows you to check if a Sound Group exists.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to check.</param>
    /// <returns>Whether or not the Sound Group exists.</returns>
    public static bool SoundGroupExists(string sType) {
        var aGroup = GrabGroup(sType, false);
        return aGroup != null;
    }

    /// <summary>
    /// This method allows you to pause all Audio Sources in a Sound Group.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to pause.</param>
    public static void PauseSoundGroup(string sType) {
        var aGroup = GrabGroup(sType);

        if (aGroup == null) {
            return;
        }

        SoundGroupVariation aVar = null;

        var sources = audioSourcesBySoundType[sType]._sources;

        for (var i = 0; i < sources.Count; i++) {
            aVar = sources[i].variation;

            aVar.Pause();
        }
    }

    /// <summary>
    /// This method allows you to unpause all Audio Sources in a Sound Group.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to unpause.</param>
    public static void UnpauseSoundGroup(string sType) {
        var aGroup = GrabGroup(sType);

        if (aGroup == null) {
            return;
        }

        SoundGroupVariation aVar = null;

        var sources = audioSourcesBySoundType[sType]._sources;

        for (var i = 0; i < sources.Count; i++) {
            aVar = sources[i].variation;

            if (!AudioUtil.IsAudioPaused(aVar.VarAudio)) {
                continue;
            }

            aVar.VarAudio.Play();
        }
    }

    /// <summary>
    /// This method allows you to fade the volume of a Sound Group over X seconds.
    /// </summary>
    /// <param name="sType">The name of the Sound Group to fade.</param>
    /// <param name="newVolume">The target volume of the Sound Group.</param>
    /// <param name="fadeTime">The amount of time the fade will take.</param>
    /// <param name="completionCallback">(Optional) - a method to execute when the fade has completed.</param>
    public static void FadeSoundGroupToVolume(string sType, float newVolume, float fadeTime, System.Action completionCallback = null) {
		if (newVolume < 0f || newVolume > 1f) {
			Debug.LogError("Illegal volume passed to FadeSoundGroupToVolume: '" + newVolume + "'. Legal volumes are between 0 and 1");
			return;
		}

		if (fadeTime <= INNER_LOOP_CHECK_INTERVAL) {
            SetGroupVolume(sType, newVolume); // time really short, just do it at once.
            return;
        }

        var aGroup = GrabGroup(sType);

        if (aGroup == null) {
            return;
        }

        if (newVolume < 0f || newVolume > 1f) {
            Debug.Log("Cannot fade Sound Group '" + sType + "'. Invalid volume specified. Volume should be between 0 and 1.");
            return;
        }

        // make sure no other group fades for this group are happenning.
        var matchingFade = groupFades.Find(delegate(GroupFadeInfo obj) {
            return obj.NameOfGroup == sType;
        });

        if (matchingFade != null) {
            matchingFade.IsActive = false; // start with a new one, delete old.
        }

        var volStep = (newVolume - aGroup.groupMasterVolume) / (fadeTime / INNER_LOOP_CHECK_INTERVAL);

        var groupFade = new GroupFadeInfo() {
            NameOfGroup = sType,
            ActingGroup = aGroup,
            VolumeStep = volStep,
            TargetVolume = newVolume
        };

        if (completionCallback != null) {
            groupFade.completionAction = completionCallback;
        }

        groupFades.Add(groupFade);
    }

    /// <summary>
    /// This method will delete a Sound Group, and all variations from the current Scene's Master Audio object.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    public static void RemoveSoundGroup(Transform groupTrans) {
        string sType = groupTrans.name;

        var ma = MasterAudio.Instance;

        if (ma.duckingBySoundType.ContainsKey(sType)) {
            ma.duckingBySoundType.Remove(sType);
        }

        randomizer.Remove(sType);
		randomizerLeftovers.Remove(sType);
		clipsPlayedBySoundTypeOldestFirst.Remove(sType);
        audioSourcesBySoundType.Remove(sType);
        lastTimeSoundGroupPlayed.Remove(sType);

        AudioSource aSource = null;
        SoundGroupVariation aVar = null;
        Transform aChild = null;

        // delete resource file pointers to Audio Sources being deleted
        for (var i = 0; i < groupTrans.childCount; i++) {
            aChild = groupTrans.GetChild(i);
			aSource = aChild.GetComponent<AudioSource>();
            aVar = aChild.GetComponent<SoundGroupVariation>();

            if (aVar.audLocation != MasterAudio.AudioLocation.ResourceFile) {
                continue;
            }

            AudioResourceOptimizer.DeleteAudioSourceFromList(aVar.resourceFileName, aSource);
        }

        groupTrans.parent = null;
        GameObject.Destroy(groupTrans.gameObject);
    }

    /// <summary>
    /// This method will create a new Sound Group from the Audio Clips you pass in.
    /// </summary>
    /// <param name="gInfo">The object containing all variations and group info.</param>
    /// <param name="creatorObjectName">The name of the object creating this group (for debug).</param>
	/// <param name="errorOnExisting">Whether to log an error if the Group already exists (same name).</param>
	/// <returns>Whether or not the Sound Group was created.</returns>
    public static Transform CreateNewSoundGroup(DynamicSoundGroup aGroup, string creatorObjectName, bool errorOnExisting = true) {
        if (!SceneHasMasterAudio) {
            return null;
        }

        if (!SoundsReady) {
            Debug.LogError("MasterAudio not finished initializing sounds. Cannot create new group yet.");
            return null;
        }

        var groupName = aGroup.transform.name;

        var ma = MasterAudio.Instance;

        if (MasterAudio.Instance.Trans.FindChild(groupName) != null) {
            if (errorOnExisting) {
				Debug.LogError("Cannot add a new Sound Group named '" + groupName + "' because there is already a Sound Group of that name.");
			}
            return null;
        }

        GameObject newGroup = (GameObject)GameObject.Instantiate(
            ma.soundGroupTemplate.gameObject, ma.Trans.position, Quaternion.identity);

        var groupTrans = newGroup.transform;
        groupTrans.name = UtilStrings.TrimSpace(groupName);
        groupTrans.parent = MasterAudio.Instance.Trans;

        SoundGroupVariation variation = null;
        DynamicGroupVariation aVariation = null;
        AudioClip clip = null;

        for (var i = 0; i < aGroup.groupVariations.Count; i++) {
            aVariation = aGroup.groupVariations[i];

            for (var j = 0; j < aVariation.weight; j++) {
                GameObject newVariation = (GameObject)GameObject.Instantiate(aVariation.gameObject, groupTrans.position, Quaternion.identity);
                newVariation.transform.parent = groupTrans;

                // remove dynamic group variation script.
                GameObject.Destroy(newVariation.GetComponent<DynamicGroupVariation>());

                newVariation.AddComponent<SoundGroupVariation>();
                variation = newVariation.GetComponent<SoundGroupVariation>();

                var clipName = variation.name;
                var cloneIndex = clipName.IndexOf("(Clone)");
                if (cloneIndex >= 0) {
                    clipName = clipName.Substring(0, cloneIndex);
                }

				var aVarAudio = aVariation.GetComponent<AudioSource>();

                switch (aVariation.audLocation) {
                    case MasterAudio.AudioLocation.Clip:
						clip = aVarAudio.clip;
                        if (clip == null) {
                            Debug.LogWarning("No clip specified in DynamicSoundGroupCreator '" + creatorObjectName + "' - skipping this item.");
                            continue;
                        }
                        variation.VarAudio.clip = clip;
                        break;
                    case MasterAudio.AudioLocation.ResourceFile:
                        AudioResourceOptimizer.AddTargetForClip(aVariation.resourceFileName, variation.VarAudio);
                        variation.audLocation = MasterAudio.AudioLocation.ResourceFile;
                        variation.resourceFileName = aVariation.resourceFileName;
                        variation.useLocalization = aVariation.useLocalization;
                        break;
                }

                variation.original_pitch = aVarAudio.pitch;
				variation.transform.name = clipName;
				variation.isExpanded = aVariation.isExpanded;

                variation.useRandomPitch = aVariation.useRandomPitch;
                variation.randomPitchMode = aVariation.randomPitchMode;
                variation.randomPitchMin = aVariation.randomPitchMin;
                variation.randomPitchMax = aVariation.randomPitchMax;

                variation.useRandomVolume = aVariation.useRandomVolume;
                variation.randomVolumeMode = aVariation.randomVolumeMode;
                variation.randomVolumeMin = aVariation.randomVolumeMin;
                variation.randomVolumeMax = aVariation.randomVolumeMax;

                variation.useFades = aVariation.useFades;
                variation.fadeInTime = aVariation.fadeInTime;
                variation.fadeOutTime = aVariation.fadeOutTime;

                variation.useIntroSilence = aVariation.useIntroSilence;
                variation.introSilenceMin = aVariation.introSilenceMin;
                variation.introSilenceMax = aVariation.introSilenceMax;
                variation.fxTailTime = aVariation.fxTailTime;

                // remove unused filter FX
                if (variation.LowPassFilter != null && !variation.LowPassFilter.enabled) {
                    GameObject.Destroy(variation.LowPassFilter);
                }
                if (variation.HighPassFilter != null && !variation.HighPassFilter.enabled) {
                    GameObject.Destroy(variation.HighPassFilter);
                }
                if (variation.DistortionFilter != null && !variation.DistortionFilter.enabled) {
                    GameObject.Destroy(variation.DistortionFilter);
                }
                if (variation.ChorusFilter != null && !variation.ChorusFilter.enabled) {
                    GameObject.Destroy(variation.ChorusFilter);
                }
                if (variation.EchoFilter != null && !variation.EchoFilter.enabled) {
                    GameObject.Destroy(variation.EchoFilter);
                }
                if (variation.ReverbFilter != null && !variation.ReverbFilter.enabled) {
                    GameObject.Destroy(variation.ReverbFilter);
                }
            }
        }
        // added to Hierarchy!

        // populate sounds for playing!
        var groupScript = newGroup.GetComponent<MasterAudioGroup>();
        // populate other properties.
        groupScript.retriggerPercentage = aGroup.retriggerPercentage;
        groupScript.groupMasterVolume = aGroup.groupMasterVolume;
        groupScript.limitMode = aGroup.limitMode;
        groupScript.limitPerXFrames = aGroup.limitPerXFrames;
        groupScript.minimumTimeBetween = aGroup.minimumTimeBetween;
        groupScript.limitPolyphony = aGroup.limitPolyphony;
        groupScript.voiceLimitCount = aGroup.voiceLimitCount;
        groupScript.curVariationSequence = aGroup.curVariationSequence;
        groupScript.useInactivePeriodPoolRefill = aGroup.useInactivePeriodPoolRefill;
        groupScript.inactivePeriodSeconds = aGroup.inactivePeriodSeconds;
        groupScript.curVariationMode = aGroup.curVariationMode;
        groupScript.useDialogFadeOut = aGroup.useDialogFadeOut;
        groupScript.dialogFadeOutTime = aGroup.dialogFadeOutTime;

        groupScript.chainLoopDelayMin = aGroup.chainLoopDelayMin;
        groupScript.chainLoopDelayMax = aGroup.chainLoopDelayMax;
        groupScript.chainLoopMode = aGroup.chainLoopMode;
        groupScript.chainLoopNumLoops = aGroup.chainLoopNumLoops;

        groupScript.childGroupMode = aGroup.childGroupMode;
        groupScript.childSoundGroups = aGroup.childSoundGroups;

		groupScript.targetDespawnedBehavior = aGroup.targetDespawnedBehavior;
		groupScript.despawnFadeTime = aGroup.despawnFadeTime;

        groupScript.resourceClipsAllLoadAsync = aGroup.resourceClipsAllLoadAsync;
        groupScript.logSound = aGroup.logSound;
		groupScript.alwaysHighestPriority = aGroup.alwaysHighestPriority;

        var sources = new List<AudioInfo>();
        Transform aChild;
        AudioSource aSource;
        var playedStatuses = new List<int>();

        for (var i = 0; i < newGroup.transform.childCount; i++) {
            playedStatuses.Add(i);
            aChild = newGroup.transform.GetChild(i);
            aSource = aChild.GetComponent<AudioSource>();
            variation = aChild.GetComponent<SoundGroupVariation>();
            sources.Add(new AudioInfo(variation, aSource, aSource.volume));

            if (variation.VariationUpdater != null) {
                variation.DisableUpdater();
            }
        }

        audioSourcesBySoundType.Add(groupName, new AudioGroupInfo(sources, groupScript));

        // fill up randomizer
        randomizer.Add(groupName, playedStatuses);
		randomizerLeftovers.Add (groupName, new List<int> (playedStatuses.Count));
        clipsPlayedBySoundTypeOldestFirst.Add(groupName, new List<int>(playedStatuses.Count));

        if (!string.IsNullOrEmpty(aGroup.busName)) {
            groupScript.busIndex = GetBusIndex(aGroup.busName, true);
        }

        return groupTrans;
    }

    /// <summary>
    /// This will return the volume of a Sound Group.
    /// </summary>
    /// <param name="sType">The name of the Sound Group</param>
    /// <returns>The volume of the Sound Group</returns>
    public static float GetGroupVolume(string sType) {
        var aGroup = GrabGroup(sType);
        if (aGroup == null) {
            return 0f;
        }

        return aGroup.groupMasterVolume;
    }

    /// <summary>
    /// This method will set the volume of a Sound Group.
    /// </summary>
    /// <param name="sType">The name of the Sound Group</param>
    /// <param name="volumeLevel">The new volume level.</param>
    public static void SetGroupVolume(string sType, float volumeLevel) {
        var aGroup = GrabGroup(sType, Application.isPlaying);
        if (aGroup == null || AppIsShuttingDown) {
            return;
        }

        aGroup.groupMasterVolume = volumeLevel;

        AudioInfo aInfo;
        AudioSource aSource = null;

        var theGroup = audioSourcesBySoundType[sType];

        var busVolume = GetBusVolume(aGroup);

        for (var i = 0; i < theGroup._sources.Count; i++) {
            aInfo = theGroup._sources[i];
            aSource = aInfo.source;

            if (aSource == null) {
                continue;
            }

            var newVol = 0f;
            if (aInfo.variation.randomVolumeMode == SoundGroupVariation.RandomVolumeMode.AddToClipVolume) {
                newVol = (aInfo.originalVolume * aInfo.lastPercentageVolume * aGroup.groupMasterVolume * busVolume * MasterAudio.Instance.masterAudioVolume) + aInfo.lastRandomVolume;
            } else { // ignore original volume
                newVol = (aInfo.originalVolume * aInfo.lastPercentageVolume * aGroup.groupMasterVolume * busVolume * MasterAudio.Instance.masterAudioVolume);
            }
            aSource.volume = newVol;
        }
    }

    /// <summary>
    /// This method will mute all variations in a Sound Group.
    /// </summary>
    /// <param name="sType">The name of the Sound Group</param>
    public static void MuteGroup(string sType) {
        var aGroup = GrabGroup(sType);
        if (aGroup == null) {
            return;
        }

        soloedGroups.Remove(aGroup);
        aGroup.isSoloed = false;

        SetGroupMuteStatus(aGroup, sType, true);
    }

    /// <summary>
    /// This method will unmute all variations in a Sound Group
    /// </summary>
    /// <param name="sType">The name of the Sound Group</param>
    public static void UnmuteGroup(string sType) {
        var aGroup = GrabGroup(sType);
        if (aGroup == null) {
            return;
        }

        SetGroupMuteStatus(aGroup, sType, false);
    }

    private static void SetGroupMuteStatus(MasterAudioGroup aGroup, string sType, bool isMute) {
        aGroup.isMuted = isMute;

        var theGroup = audioSourcesBySoundType[sType];
        AudioInfo aInfo;
        AudioSource aSource;

        for (var i = 0; i < theGroup._sources.Count; i++) {
            aInfo = theGroup._sources[i];
            aSource = aInfo.source;

            aSource.mute = isMute;
        }
    }

    /// <summary>
    /// This method will solo a Sound Group. If anything is soloed, only soloed Sound Groups will be heard.
    /// </summary>
    /// <param name="sType">The name of the Sound Group</param>
    public static void SoloGroup(string sType) {
        var aGroup = GrabGroup(sType);
        if (aGroup == null) {
            return;
        }

        aGroup.isMuted = false;
        aGroup.isSoloed = true;

        soloedGroups.Add(aGroup);

        SetGroupMuteStatus(aGroup, sType, false);
    }

    /// <summary>
    /// This method will unsolo a Sound Group. 
    /// </summary>
    /// <param name="sType">The name of the Sound Group</param>
    public static void UnsoloGroup(string sType) {
        var aGroup = GrabGroup(sType);
        if (aGroup == null) {
            return;
        }

        aGroup.isSoloed = false;

        soloedGroups.Remove(aGroup);
    }

    /// <summary>
    /// This method will return the Sound Group settings for examination purposes.
    /// </summary>
    /// <param name="sType">The name of the Sound Group</param>
    /// <returns>A MasterAudioGroup object</returns>
    public static MasterAudioGroup GrabGroup(string sType, bool logIfMissing = true) {
        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            if (logIfMissing) {
                Debug.LogError("Could not grab Sound Group '" + sType + "' because it does not exist in this scene.");
            }
            return null;
        }

        AudioGroupInfo _group = audioSourcesBySoundType[sType];
        MasterAudioGroup maGroup = _group._group;
        return maGroup;
    }

    /// <summary>
    /// This method will return the Audio Group Info settings for examination purposes. Use on during play in editor, not during edit.
    /// </summary>
    /// <param name="sType">The name of the Sound Group</param>
    /// <returns>an Audio Group Info object</returns>
    public static AudioGroupInfo GetGroupInfo(string sType) {
        if (!audioSourcesBySoundType.ContainsKey(sType)) {
            return null;
        }

        AudioGroupInfo _group = audioSourcesBySoundType[sType];
        return _group;
    }
    #endregion

    #region Mixer methods
    /// <summary>
    /// This method allows you to pause all Audio Sources in the mixer (everything but Playlists).
    /// </summary>
    public static void PauseMixer() {
        foreach (var key in audioSourcesBySoundType.Keys) {
            PauseSoundGroup(audioSourcesBySoundType[key]._group.GameObjectName);
        }
    }

    /// <summary>
    /// This method allows you to unpause all Audio Sources in the mixer (everything but Playlists).
    /// </summary>
    public static void UnpauseMixer() {
        foreach (var key in audioSourcesBySoundType.Keys) {
            UnpauseSoundGroup(audioSourcesBySoundType[key]._group.GameObjectName);
        }
    }

    /// <summary>
    /// This method allows you to stop all Audio Sources in the mixer (everything but Playlists).
    /// </summary>
    public static void StopMixer() {
        foreach (var key in audioSourcesBySoundType.Keys) {
			StopAllOfSound(audioSourcesBySoundType[key]._group.GameObjectName);
        }
    }
    #endregion

    #region Global Controls
    /// <summary>
    /// This method allows you to stop all Audio Sources in the mixer and Playlists as well.
    /// </summary>
    public static void StopEverything() {
        StopMixer();
        StopAllPlaylists();
    }

    /// <summary>
    /// This method allows you to pause all Audio Sources in the mixer and Playlists as well.
    /// </summary>
    public static void PauseEverything() {
        PauseMixer();
        PauseAllPlaylists();
    }

    /// <summary>
    /// This method allows you to unpause all Audio Sources in the mixer and Playlists as well.
    /// </summary>
    public static void UnpauseEverything() {
        UnpauseMixer();
        ResumeAllPlaylists();
    }

    /// <summary>
    /// This method allows you to mute all Audio Sources in the mixer and Playlists as well.
    /// </summary>
    public static void MuteEverything() {
        MixerMuted = true;
        MuteAllPlaylists();
    }

    /// <summary>
    /// This method allows you to unmute all Audio Sources in the mixer and Playlists as well.
    /// </summary>
    public static void UnmuteEverything() {
        MixerMuted = false;
        UnmuteAllPlaylists();
    }

    #endregion

    #region Bus methods
    private static int GetBusIndex(string busName, bool alertMissing) {
        for (var i = 0; i < GroupBuses.Count; i++) {
            if (GroupBuses[i].busName == busName) {
                return i + HARD_CODED_BUS_OPTIONS;
            }
        }

        if (alertMissing) {
            LogWarning("Could not find bus '" + busName + "'.");
        }

        return -1;
    }

    private static GroupBus GetBusByIndex(int busIndex) {
        if (busIndex < HARD_CODED_BUS_OPTIONS) {
            return null;
        }

        return GroupBuses[busIndex - HARD_CODED_BUS_OPTIONS];
    }

    /// <summary>
    /// This method allows you to change the pitch of all Variations in all Groups in a bus.
    /// </summary>
    /// <param name="busName">The name of the bus.</param>
    /// <param name="pitch">The new pitch to use.</param>
    public static void ChangeBusPitch(string busName, float pitch) {
        var busIndex = GetBusIndex(busName, true);

        if (busIndex < 0) {
            return;
        }

        var sources = audioSourcesBySoundType.GetEnumerator();

        MasterAudioGroup aGroup = null;
        AudioGroupInfo aInfo = null;

        while (sources.MoveNext()) {
            aInfo = sources.Current.Value;
            aGroup = aInfo._group;
            if (aGroup.busIndex != busIndex) {
                continue;
            }

            ChangeVariationPitch(aGroup.GameObjectName, true, string.Empty, pitch);
        }
    }

    /// <summary>
    /// This method allows you to mute all Groups in a bus.
    /// </summary>
    /// <param name="busName">The name of the bus to mute.</param>
    public static void MuteBus(string busName) {
        var busIndex = GetBusIndex(busName, true);

        if (busIndex < 0) {
            return;
        }

        var bus = GrabBusByName(busName);
        bus.isMuted = true;

        if (bus.isSoloed) {
            UnsoloBus(busName);
        }

        var sources = audioSourcesBySoundType.GetEnumerator();

        MasterAudioGroup aGroup = null;
        AudioGroupInfo aInfo = null;

        while (sources.MoveNext()) {
            aInfo = sources.Current.Value;
            aGroup = aInfo._group;
            if (aGroup.busIndex != busIndex) {
                continue;
            }

            MuteGroup(aGroup.GameObjectName);
        }
    }

    /// <summary>
    /// This method allows you to unmute all Groups in a bus.
    /// </summary>
    /// <param name="busName">The name of the bus to unmute.</param>
    public static void UnmuteBus(string busName) {
        var busIndex = GetBusIndex(busName, true);

        if (busIndex < 0) {
            return;
        }

        var bus = GrabBusByName(busName);
        bus.isMuted = false;

        var sources = audioSourcesBySoundType.GetEnumerator();

        MasterAudioGroup aGroup = null;
        AudioGroupInfo aInfo = null;

        while (sources.MoveNext()) {
            aInfo = sources.Current.Value;
            aGroup = aInfo._group;
            if (aGroup.busIndex != busIndex) {
                continue;
            }

            UnmuteGroup(aGroup.GameObjectName);
        }
    }

    /// <summary>
    /// This method allows you to solo all Groups in a bus.
    /// </summary>
    /// <param name="busName">The name of the bus to solo.</param>
    public static void SoloBus(string busName) {
        var busIndex = GetBusIndex(busName, true);

        if (busIndex < 0) {
            return;
        }

        var bus = GrabBusByName(busName);
        bus.isSoloed = true;

        if (bus.isMuted) {
            UnmuteBus(busName);
        }

        var sources = audioSourcesBySoundType.GetEnumerator();

        MasterAudioGroup aGroup = null;
        AudioGroupInfo aInfo = null;

        while (sources.MoveNext()) {
            aInfo = sources.Current.Value;
            aGroup = aInfo._group;
            if (aGroup.busIndex != busIndex) {
                continue;
            }

            SoloGroup(aGroup.GameObjectName);
        }
    }

    /// <summary>
    /// This method allows you to unsolo all Groups in a bus.
    /// </summary>
    /// <param name="busName">The name of the bus to unsolo.</param>
    public static void UnsoloBus(string busName) {
        var busIndex = GetBusIndex(busName, true);

        if (busIndex < 0) {
            return;
        }

        var bus = GrabBusByName(busName);
        bus.isSoloed = false;

        var sources = audioSourcesBySoundType.GetEnumerator();

        MasterAudioGroup aGroup = null;
        AudioGroupInfo aInfo = null;

        while (sources.MoveNext()) {
            aInfo = sources.Current.Value;
            aGroup = aInfo._group;
            if (aGroup.busIndex != busIndex) {
                continue;
            }

            UnsoloGroup(aGroup.GameObjectName);
        }
    }

    /// <summary>
    /// This method allows you to pause all Audio Sources in a bus.
    /// </summary>
    /// <param name="busName">The name of the bus to pause.</param>
    public static void PauseBus(string busName) {
        var busIndex = GetBusIndex(busName, true);

        if (busIndex < 0) {
            return;
        }

        var sources = audioSourcesBySoundType.GetEnumerator();

        MasterAudioGroup aGroup = null;
        AudioGroupInfo aInfo = null;

        while (sources.MoveNext()) {
            aInfo = sources.Current.Value;
            aGroup = aInfo._group;
            if (aGroup.busIndex != busIndex) {
                continue;
            }

            PauseSoundGroup(aGroup.GameObjectName);
        }
    }

    /// <summary>
    /// This method allows you to stop all Audio Sources in a bus.
    /// </summary>
    /// <param name="busName">The name of the bus to stop.</param>
    public static void StopBus(string busName) {
        var busIndex = GetBusIndex(busName, true);

        if (busIndex < 0) {
            return;
        }

        var sources = audioSourcesBySoundType.GetEnumerator();

        MasterAudioGroup aGroup = null;
        AudioGroupInfo aInfo = null;

        while (sources.MoveNext()) {
            aInfo = sources.Current.Value;
            aGroup = aInfo._group;
            if (aGroup.busIndex != busIndex) {
                continue;
            }

            StopAllOfSound(aGroup.GameObjectName);
        }
    }

    /// <summary>
    /// This method allows you to unpause all paused Audio Sources in a bus.
    /// </summary>
    /// <param name="busName">The name of the bus to unpause.</param>
    public static void UnpauseBus(string busName) {
        var busIndex = GetBusIndex(busName, true);

        if (busIndex < 0) {
            return;
        }

        var sources = audioSourcesBySoundType.GetEnumerator();

        MasterAudioGroup aGroup = null;
        AudioGroupInfo aInfo = null;

        while (sources.MoveNext()) {
            aInfo = sources.Current.Value;
            aGroup = aInfo._group;
            if (aGroup.busIndex != busIndex) {
                continue;
            }

            UnpauseSoundGroup(aGroup.GameObjectName);
        }
    }

    /// <summary>
    /// This method will create a new bus with the name you specify.
    /// </summary>
    /// <param name="busName">The name of the new bus.</param>
	/// <param name="errorOnExisting">Whether to log an error if the bus already exists (same name).</param>
	public static bool CreateBus(string busName, bool errorOnExisting = true) {
        var match = GroupBuses.FindAll(delegate(GroupBus obj) {
            return obj.busName == busName;
        });

        if (match.Count > 0) {
			if (errorOnExisting) {
				LogError("You already have a bus named '" + busName + "'. Not creating a second one.");
			}
            return false;
        }

        var newBus = new GroupBus();
        newBus.busName = busName;

        GroupBuses.Add(newBus);
        return true;
    }

    /// <summary>
    /// This method will delete a bus by name.
    /// </summary>
    /// <param name="busName">The name of the bus to delete.</param>
    public static void DeleteBusByName(string busName) {
        var index = GetBusIndex(busName, false);
        if (index > 0) {
            DeleteBus(index);
        }
    }

    public static void DeleteBus(int busIndex) {
        GroupBuses.RemoveAt(busIndex - HARD_CODED_BUS_OPTIONS);

        MasterAudioGroup aGroup = null;

        var sources = audioSourcesBySoundType.GetEnumerator();

        while (sources.MoveNext()) {
            aGroup = sources.Current.Value._group;
            if (aGroup.busIndex == -1) {
                continue;
            }
            if (aGroup.busIndex == busIndex + MasterAudio.HARD_CODED_BUS_OPTIONS) {
                aGroup.busIndex = -1;
            } else if (aGroup.busIndex > busIndex + MasterAudio.HARD_CODED_BUS_OPTIONS) {
                aGroup.busIndex--;
            }
        }
    }

    /// <summary>
    /// This method will return the bus volume of a specified Sound Group, if any. If the Group is not in a bus, this will return 1.
    /// </summary>
    /// <param name="maGroup">The Sound Group object.</param>
    /// <returns>The volume of the bus.</returns>
    public static float GetBusVolume(MasterAudioGroup maGroup) {
        var busVolume = 1f;
        if (maGroup.busIndex >= HARD_CODED_BUS_OPTIONS) {
            busVolume = GroupBuses[maGroup.busIndex - HARD_CODED_BUS_OPTIONS].volume;
        }

        return busVolume;
    }

    /// <summary>
    /// This method allows you to fade the volume of a bus over X seconds.
    /// </summary>
    /// <param name="busName">The name of the bus to fade.</param>
    /// <param name="newVolume">The target volume of the bus.</param>
    /// <param name="fadeTime">The amount of time the fade will take.</param>
    /// <param name="completionCallback">(Optional) - a method to execute when the fade has completed.</param>
    public static void FadeBusToVolume(string busName, float newVolume, float fadeTime, System.Action completionCallback = null) {
		if (newVolume < 0f || newVolume > 1f) {
			Debug.LogError("Illegal volume passed to FadeBusToVolume: '" + newVolume + "'. Legal volumes are between 0 and 1");
			return;
		}

		if (fadeTime <= INNER_LOOP_CHECK_INTERVAL) {
            SetBusVolumeByName(busName, newVolume); // time really short, just do it at once.
            return;
        }

        var bus = GrabBusByName(busName);

        if (bus == null) {
            Debug.Log("Could not find bus '" + busName + "' to fade it.");
            return;
        }

        if (newVolume < 0f || newVolume > 1f) {
            Debug.Log("Cannot fade bus '" + busName + "'. Invalid volume specified. Volume should be between 0 and 1.");
            return;
        }

        // make sure no other bus fades for this bus are happenning.
        var matchingFade = busFades.Find(delegate(BusFadeInfo obj) {
            return obj.NameOfBus == busName;
        });

        if (matchingFade != null) {
            matchingFade.IsActive = false; // start with a new one, delete old.
        }

        var volStep = (newVolume - bus.volume) / (fadeTime / INNER_LOOP_CHECK_INTERVAL);

        var busFade = new BusFadeInfo() {
            NameOfBus = busName,
            ActingBus = bus,
            VolumeStep = volStep,
            TargetVolume = newVolume
        };

        if (completionCallback != null) {
            busFade.completionAction = completionCallback;
        }

        busFades.Add(busFade);
    }

    /// <summary>
    /// This method will set the volume of a bus.
    /// </summary>
    /// <param name="newVolume">The volume to set the bus to.</param>
    /// <param name="busName">The bus name.</param>
    public static void SetBusVolumeByName(string busName, float newVolume) {
        var bus = GrabBusByName(busName);
        if (bus == null) {
            Debug.LogError("bus '" + busName + "' not found!");
            return;
        }

        SetBusVolume(bus, newVolume);
    }

    private static void SetBusVolume(GroupBus bus, float newVolume) {
        bus.volume = newVolume;

        AudioInfo aInfo;
        AudioSource aSource = null;
        AudioGroupInfo aGroup = null;

        foreach (var key in audioSourcesBySoundType.Keys) {
            aGroup = audioSourcesBySoundType[key];
            var groupBus = GetBusByIndex(aGroup._group.busIndex);

            if (groupBus == null || groupBus.busName != bus.busName) {
                continue;
            }

            for (var i = 0; i < aGroup._sources.Count; i++) {
                aInfo = aGroup._sources[i];
                aSource = aInfo.source;

                if (!aSource.isPlaying) {
                    continue;
                }

                var newVol = (aInfo.originalVolume * aInfo.lastPercentageVolume * aGroup._group.groupMasterVolume * bus.volume * MasterAudio.Instance.masterAudioVolume) + aInfo.lastRandomVolume;
                aSource.volume = newVol;
            }
        }
    }

    /// <summary>
    /// This method will return the settings of a bus.
    /// </summary>
    /// <param name="busName">The bus name.</param>
    /// <returns>GroupBus object</returns>
    public static GroupBus GrabBusByName(string busName) {
        for (var i = 0; i < GroupBuses.Count; i++) {
            var aBus = GroupBuses[i];
            if (aBus.busName == busName) {
                return aBus;
            }
        }

        return null;
    }

    #endregion

    #region Ducking methods
    /// <summary>
    /// This method will allow you to add a Sound Group to the list of sounds that cause music in the Playlist to duck.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    public static void AddSoundGroupToDuckList(string sType, float riseVolumeStart) {
        var ma = MasterAudio.Instance;

        if (ma.duckingBySoundType.ContainsKey(sType)) {
            return;
        }

        ma.duckingBySoundType.Add(sType, new DuckGroupInfo() {
            soundType = sType,
            riseVolStart = riseVolumeStart
        });
    }

    /// <summary>
    /// This method will allow you to remove a Sound Group from the list of sounds that cause music in the Playlist to duck.
    /// </summary>
    /// <param name="sType">The name of the Sound Group.</param>
    public static void RemoveSoundGroupFromDuckList(string sType) {
        var ma = MasterAudio.Instance;

        if (!ma.duckingBySoundType.ContainsKey(sType)) {
            return;
        }

        ma.duckingBySoundType.Remove(sType);
    }
    #endregion

    #region Playlist methods
    /// <summary>
    /// This method will find a Playlist by name and return it to you.
    /// </summary>
    public static Playlist GrabPlaylist(string playlistName, bool logErrorIfNotFound = true) {
        if (playlistName == MasterAudio.NO_GROUP_NAME) {
            return null;
        }

        for (var i = 0; i < MusicPlaylists.Count; i++) {
            var aPlaylist = MusicPlaylists[i];
            if (aPlaylist.playlistName == playlistName) {
                return aPlaylist;
            }
        }

        if (logErrorIfNotFound) {
            Debug.LogError("Could not find Playlist '" + playlistName + "'.");
        }

        return null;
    }

    /// <summary>
    /// This method will change the pitch of all clips in a Playlist, or a single song if you specify the song name.
    /// </summary>
    /// <param name="playlistName">The name of the Playlist.</param>
    /// <param name="pitch">The pitch to change the songs to.</param>
	/// <param name="songName">(Optional) the song name to change the pitch of. If not specified, all songs will be changed.</param>
	public static void ChangePlaylistPitch(string playlistName, float pitch, string songName = null) {
        var playlist = GrabPlaylist(playlistName);

        if (playlist == null) {
            return;
        }

        for (var i = 0; i < playlist.MusicSettings.Count; i++) {
            var aSong = playlist.MusicSettings[i];
            
			if (!string.IsNullOrEmpty(songName) && aSong.songName != songName) {
				continue;
			}
				
			aSong.pitch = pitch;
        }
    }

    #region Mute Playlist
    /// <summary>
    /// This method will allow you to mute your Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    public static void MutePlaylist() {
        MutePlaylist(ONLY_PLAYLIST_CONTROLLER_NAME);
    }

    /// <summary>
    /// This method will allow you to mute a Playlist Controller by name.
    /// </summary>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    public static void MutePlaylist(string playlistControllerName) {
        var pcs = PlaylistController.Instances;

        var controllers = new List<PlaylistController>();

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "PausePlaylist")) {
                return;
            }

            controllers.Add(pcs[0]);
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl != null) {
                controllers.Add(pl);
            }
        }

        MutePlaylists(controllers);
    }

    /// <summary>
    /// This method will allow you to mute all Playlist Controllers.
    /// </summary>
    public static void MuteAllPlaylists() {
        MutePlaylists(PlaylistController.Instances);
    }

    private static void MutePlaylists(List<PlaylistController> playlists) {
        if (playlists.Count == PlaylistController.Instances.Count) {
            PlaylistsMuted = true;
        }

        PlaylistController aList = null;

        for (var i = 0; i < playlists.Count; i++) {
            aList = playlists[i];
            aList.MutePlaylist();
        }
    }
    #endregion

    #region Unmute Playlist
    /// <summary>
    /// This method will allow you to unmute your Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    public static void UnmutePlaylist() {
        UnmutePlaylist(ONLY_PLAYLIST_CONTROLLER_NAME);
    }

    /// <summary>
    /// This method will allow you to unmute a Playlist Controller by name.
    /// </summary>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    public static void UnmutePlaylist(string playlistControllerName) {
        var pcs = PlaylistController.Instances;

        var controllers = new List<PlaylistController>();

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "PausePlaylist")) {
                return;
            }

            controllers.Add(pcs[0]);
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl != null) {
                controllers.Add(pl);
            }
        }

        UnmutePlaylists(controllers);
    }

    /// <summary>
    /// This method will allow you to unmute all Playlist Controllers.
    /// </summary>
    public static void UnmuteAllPlaylists() {
        UnmutePlaylists(PlaylistController.Instances);
    }

    private static void UnmutePlaylists(List<PlaylistController> playlists) {
        if (playlists.Count == PlaylistController.Instances.Count) {
            PlaylistsMuted = false;
        }

        PlaylistController aList = null;

        for (var i = 0; i < playlists.Count; i++) {
            aList = playlists[i];
            aList.UnmutePlaylist();
        }
    }
    #endregion

    #region Toggle Mute Playlist
    /// <summary>
    /// This method will allow you to toggle mute on your Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    public static void ToggleMutePlaylist() {
        ToggleMutePlaylist(ONLY_PLAYLIST_CONTROLLER_NAME);
    }

    /// <summary>
    /// This method will allow you to toggle mute on a Playlist Controller by name.
    /// </summary>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    public static void ToggleMutePlaylist(string playlistControllerName) {
        var pcs = PlaylistController.Instances;

        var controllers = new List<PlaylistController>();

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "PausePlaylist")) {
                return;
            }

            controllers.Add(pcs[0]);
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl != null) {
                controllers.Add(pl);
            }
        }

        ToggleMutePlaylists(controllers);
    }

    /// <summary>
    /// This method will allow you to toggle mute on all Playlist Controllers.
    /// </summary>
    public static void ToggleMuteAllPlaylists() {
        ToggleMutePlaylists(PlaylistController.Instances);
    }

    private static void ToggleMutePlaylists(List<PlaylistController> playlists) {
        PlaylistController aList = null;

        for (var i = 0; i < playlists.Count; i++) {
            aList = playlists[i];
            aList.ToggleMutePlaylist();
        }
    }
    #endregion

    #region Pause Playlist
    /// <summary>
    /// This method will allow you to pause your Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    public static void PausePlaylist() {
        PausePlaylist(ONLY_PLAYLIST_CONTROLLER_NAME);
    }

    /// <summary>
    /// This method will allow you to pause a Playlist Controller by name.
    /// </summary>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    public static void PausePlaylist(string playlistControllerName) {
        var pcs = PlaylistController.Instances;

        var controllers = new List<PlaylistController>();

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "PausePlaylist")) {
                return;
            }

            controllers.Add(pcs[0]);
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl != null) {
                controllers.Add(pl);
            }
        }

        PausePlaylists(controllers);
    }

    /// <summary>
    /// This method will allow you to pause all Playlist Controllers.
    /// </summary>
    public static void PauseAllPlaylists() {
        PausePlaylists(PlaylistController.Instances);
    }

    private static void PausePlaylists(List<PlaylistController> playlists) {
        PlaylistController aList = null;

        for (var i = 0; i < playlists.Count; i++) {
            aList = playlists[i];
            aList.PausePlaylist();
        }
    }
    #endregion

    #region Resume Playlist
    /// <summary>
    /// This method will allow you to resume a paused Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    public static void ResumePlaylist() {
        ResumePlaylist(ONLY_PLAYLIST_CONTROLLER_NAME);
    }

    /// <summary>
    /// This method will allow you to resume a paused Playlist Controller by name.
    /// </summary>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    public static void ResumePlaylist(string playlistControllerName) {
        var pcs = PlaylistController.Instances;

        var controllers = new List<PlaylistController>();

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "ResumePlaylist")) {
                return;
            }

            controllers.Add(pcs[0]);
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl != null) {
                controllers.Add(pl);
            }
        }

        ResumePlaylists(controllers);
    }

    /// <summary>
    /// This method will allow you to resume all paused Playlist Controllers.
    /// </summary>
    public static void ResumeAllPlaylists() {
        ResumePlaylists(PlaylistController.Instances);
    }

    private static void ResumePlaylists(List<PlaylistController> controllers) {
        PlaylistController aList = null;

        for (var i = 0; i < controllers.Count; i++) {
            aList = controllers[i];
            aList.ResumePlaylist();
        }
    }
    #endregion

    #region Stop Playlist
    /// <summary>
    /// This method will stop a Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    public static void StopPlaylist() {
        StopPlaylist(ONLY_PLAYLIST_CONTROLLER_NAME);
    }

    /// <summary>
    /// This method will stop a Playlist Controller by name.
    /// </summary>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    public static void StopPlaylist(string playlistControllerName) {
        var pcs = PlaylistController.Instances;

        var controllers = new List<PlaylistController>();

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "StopPlaylist")) {
                return;
            }

            controllers.Add(pcs[0]);
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl != null) {
                controllers.Add(pl);
            }
        }

        StopPlaylists(controllers);
    }

    /// <summary>
    /// This method will allow you to stop all Playlist Controllers.
    /// </summary>
    public static void StopAllPlaylists() {
        StopPlaylists(PlaylistController.Instances);
    }

    private static void StopPlaylists(List<PlaylistController> playlists) {
        PlaylistController aList = null;

        for (var i = 0; i < playlists.Count; i++) {
            aList = playlists[i];
            aList.StopPlaylist();
        }
    }
    #endregion

    #region Next Playlist Clip
    /// <summary>
    /// This method will advance the Playlist to the next clip in your Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    public static void TriggerNextPlaylistClip() {
        TriggerNextPlaylistClip(ONLY_PLAYLIST_CONTROLLER_NAME);
    }

    /// <summary>
    /// This method will advance the Playlist to the next clip in the Playlist Controller you name.
    /// </summary>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    public static void TriggerNextPlaylistClip(string playlistControllerName) {
        var pcs = PlaylistController.Instances;

        var controllers = new List<PlaylistController>();

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "TriggerNextPlaylistClip")) {
                return;
            }

            controllers.Add(pcs[0]);
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl != null) {
                controllers.Add(pl);
            }
        }

        NextPlaylistClips(controllers);
    }

    /// <summary>
    /// This method will allow you to advance Playlists in all Playlist Controllers to the next clip in their Playlist.
    /// </summary>
    public static void TriggerNextClipAllPlaylists() {
        NextPlaylistClips(PlaylistController.Instances);
    }

    private static void NextPlaylistClips(List<PlaylistController> playlists) {
        PlaylistController aList = null;

        for (var i = 0; i < playlists.Count; i++) {
            aList = playlists[i];
            aList.PlayNextSong();
        }
    }
    #endregion

    #region Random Playlist Clip
    /// <summary>
    /// This method will play a random clip in the current Playlist for your Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    public static void TriggerRandomPlaylistClip() {
        TriggerRandomPlaylistClip(ONLY_PLAYLIST_CONTROLLER_NAME);
    }

    /// <summary>
    /// This method will play a random clip in the current Playlist for the Playlist Controller you name.
    /// </summary>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    public static void TriggerRandomPlaylistClip(string playlistControllerName) {
        var pcs = PlaylistController.Instances;

        var controllers = new List<PlaylistController>();

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "TriggerRandomPlaylistClip")) {
                return;
            }

            controllers.Add(pcs[0]);
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl != null) {
                controllers.Add(pl);
            }
        }

        RandomPlaylistClips(controllers);
    }

    /// <summary>
    /// This method will allow you to play a random clip in all Playlist Controllers using their currenct Playlist
    /// </summary>
    public static void TriggerRandomClipAllPlaylists() {
        RandomPlaylistClips(PlaylistController.Instances);
    }

    private static void RandomPlaylistClips(List<PlaylistController> playlists) {
        PlaylistController aList = null;

        for (var i = 0; i < playlists.Count; i++) {
            aList = playlists[i];
            aList.PlayRandomSong();
        }
    }

    #endregion

    #region RestartPlaylist
    /// <summary>
    /// This method will restart the current Playlist in the Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    public static void RestartPlaylist() {
    	RestartPlaylist(ONLY_PLAYLIST_CONTROLLER_NAME);    
    }
	
    /// <summary>
    /// This method will restart a Playlist in the Playlist Controller. 
    /// </summary>
    /// <param name="playlistName">The name of the new Playlist.</param>
    public static void RestartPlaylist(string playlistControllerName) {
        var pcs = PlaylistController.Instances;

        PlaylistController controller = null;

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "RestartPlaylist")) {
                return;
            }

            controller = pcs[0];
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl == null) {
                return;
            }

            controller = pl;
        }

        if (controller != null) {
            RestartPlaylists(new List<PlaylistController>() { controller });
        }
    }
	
    /// <summary>
    /// This method will allow you to restart all Playlists.
    /// </summary>
    public static void RestartAllPlaylists() {
        RestartPlaylists(PlaylistController.Instances);
    }

	private static void RestartPlaylists(List<PlaylistController> playlists) {
        PlaylistController aList = null;

        for (var i = 0; i < playlists.Count; i++) {
            aList = playlists[i];
            aList.RestartPlaylist();
        }
    }
	
    #endregion

	#region StartPlaylist
	/// <summary>
	/// This method is used to start a Playlist whether it's already loaded and playing or not.
	/// </summary>
	/// <param name="playlistName">The name of the Playlist to start</param>
	public static void StartPlaylist(string playlistName) {
		StartPlaylist(ONLY_PLAYLIST_CONTROLLER_NAME, playlistName);
	}
	
	/// <summary>
	/// This method is used to start a Playlist whether it's already loaded and playing or not.
	/// </summary>
	/// <param name="playlistControllerName">The name of the Playlist Controller to use</param>
	/// <param name="playlistName">The name of the Playlist to start</param>
	public static void StartPlaylist(string playlistControllerName, string playlistName) {
		var pcs = PlaylistController.Instances;
		
		var controllers = new List<PlaylistController>();
		
		if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
			if (!IsOkToCallOnlyPlaylistMethod(pcs, "PausePlaylist")) {
				return;
			}
			
			controllers.Add(pcs[0]);
		} else {
			// multiple playlist controllers
			var pl = PlaylistController.InstanceByName(playlistControllerName);
			if (pl != null) {
				controllers.Add(pl);
			}
		}
		
		for (var i = 0; i < controllers.Count; i++) {
			controllers[i].StartPlaylist(playlistName);
		}
	}
	#endregion

    #region Queue Clip
    /// <summary>
    /// This method will play an Audio Clip by name that's in the current Playlist of your Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter. This requires auto-advance to work.
    /// </summary>
    /// <param name="clipName">The name of the clip.</param>
    public static void QueuePlaylistClip(string clipName) {
        QueuePlaylistClip(ONLY_PLAYLIST_CONTROLLER_NAME, clipName);
    }

    /// <summary>
    /// This method will play an Audio Clip by name that's in the current Playlist of the Playlist Controller you name, as soon as the currently playing song is over. Loop will be turned off on the current song. This requires auto-advance to work.
    /// </summary>
    /// <param name="clipName">The name of the clip.</param>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    public static void QueuePlaylistClip(string playlistControllerName, string clipName) {
        var pcs = PlaylistController.Instances;

        PlaylistController controller = null;

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
			if (!IsOkToCallOnlyPlaylistMethod(pcs, "QueuePlaylistClip")) {
                return;
            }

            controller = pcs[0];
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl == null) {
                return;
            }

            controller = pl;
        }

        if (controller != null) {
            controller.QueuePlaylistClip(clipName);
        }
    }

    #endregion

    #region Trigger Playlist Clip
    /// <summary>
    /// This method will play an Audio Clip by name that's in the current Playlist of your Playlist Controller. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    /// <param name="clipName">The name of the clip.</param>
    /// <returns>bool - whether the song was played or not.</returns>
    public static bool TriggerPlaylistClip(string clipName) {
        return TriggerPlaylistClip(ONLY_PLAYLIST_CONTROLLER_NAME, clipName);
    }

    /// <summary>
    /// This method will play an Audio Clip by name that's in the current Playlist of the Playlist Controller you name.
    /// </summary>
    /// <param name="clipName">The name of the clip.</param>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    /// <returns>bool - whether the song was played or not.</returns>
    public static bool TriggerPlaylistClip(string playlistControllerName, string clipName) {
        var pcs = PlaylistController.Instances;

        PlaylistController controller = null;

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "TriggerPlaylistClip")) {
                return false;
            }

            controller = pcs[0];
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl == null) {
                return false;
            }

            controller = pl;
        }

        if (controller == null) {
            return false;
        }

        return controller.TriggerPlaylistClip(clipName);
    }

    #endregion

    #region ChangePlaylistByName
    /// <summary>
    /// This method will change the current Playlist in the Playlist Controller to a Playlist whose name you specify. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    /// <param name="playlistName">The name of the new Playlist.</param>
    /// <param name="playFirstClip"><b>Optional</b> - defaults to True. If you specify false, the first clip in the Playlist will not automatically play.</param>
    public static void ChangePlaylistByName(string playlistName, bool playFirstClip = true) {
        ChangePlaylistByName(ONLY_PLAYLIST_CONTROLLER_NAME, playlistName, playFirstClip);
    }

    /// <summary>
    /// This method will play an Audio Clip by name that's in the current Playlist of the Playlist Controller you name.
    /// </summary>
    /// <param name="playlistControllerName">The Playlist Controller name</param>
    /// <param name="playlistName">The name of the new Playlist.</param>
    /// <param name="playFirstClip"><b>Optional</b> - defaults to True. If you specify false, the first clip in the Playlist will not automatically play.</param>
    public static void ChangePlaylistByName(string playlistControllerName, string playlistName, bool playFirstClip = true) {
        var pcs = PlaylistController.Instances;

        PlaylistController controller = null;

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "ChangePlaylistByName")) {
                return;
            }

            controller = pcs[0];
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl == null) {
                return;
            }

            controller = pl;
        }

        if (controller != null) {
            controller.ChangePlaylist(playlistName, playFirstClip);
        }
    }

    #endregion

    #region Playlist Fade To Volume
    /// <summary>
    /// This method will fade the volume of the Playlist Controller over X seconds. You should not use this if you have more than one Playlist Controller. Use the overloaded method instead, it takes a playlistControllerName parameter.
    /// </summary>
    /// <param name="targetVolume">The target volume of the Playlist.</param>
    /// <param name="fadeTime">The time to fade completely to the target volume.</param>
    public static void FadePlaylistToVolume(float targetVolume, float fadeTime) {
        FadePlaylistToVolume(ONLY_PLAYLIST_CONTROLLER_NAME, targetVolume, fadeTime);
    }

    /// <summary>
    /// This method will fade the volume of the Playlist Controller whose name you specify over X seconds. 
    /// </summary>
    /// <param name="playlistControllerName">The name of the Playlist Controller.</param>
    /// <param name="targetVolume">The target volume of the Playlist.</param>
    /// <param name="fadeTime">The time to fade completely to the target volume.</param>
    public static void FadePlaylistToVolume(string playlistControllerName, float targetVolume, float fadeTime) {
        var pcs = PlaylistController.Instances;

        var controllers = new List<PlaylistController>();

        if (playlistControllerName == ONLY_PLAYLIST_CONTROLLER_NAME) {
            if (!IsOkToCallOnlyPlaylistMethod(pcs, "FadePlaylistToVolume")) {
                return;
            }

            controllers.Add(pcs[0]);
        } else {
            // multiple playlist controllers
            var pl = PlaylistController.InstanceByName(playlistControllerName);
            if (pl != null) {
                controllers.Add(pl);
            }
        }

        FadePlaylists(controllers, targetVolume, fadeTime);
    }

    /// <summary>
    /// This method will allow you to fade all current Playlists used by Playlist Controllers to a target volume over X seconds.
    /// </summary>
    public static void FadeAllPlaylistsToVolume(float targetVolume, float fadeTime) {
        FadePlaylists(PlaylistController.Instances, targetVolume, fadeTime);
    }

    private static void FadePlaylists(List<PlaylistController> playlists, float targetVolume, float fadeTime) {
        if (targetVolume < 0f || targetVolume > 1f) {
			Debug.LogError("Illegal volume passed to FadePlaylistToVolume: '" + targetVolume + "'. Legal volumes are between 0 and 1");
			return;
		}

		PlaylistController aList = null;

        for (var i = 0; i < playlists.Count; i++) {
            aList = playlists[i];
            aList.FadeToVolume(targetVolume, fadeTime);
        }
    }

    #endregion


    /// <summary>
    /// This method will allow you to add a Playlist via code.
    /// </summary>
    /// <param name="Playlist">The playlist with all settings included</param>
	/// <param name="errorOnDuplicate">Whether or not to log an error if the Playlist already exists (same name).</param>
	public static void CreatePlaylist(Playlist playlist, bool errorOnDuplicate) {
        var pl = GrabPlaylist(playlist.playlistName, false);

        if (pl != null) {
            if (errorOnDuplicate) {
				Debug.LogError("You already have a Playlist Controller with the name '" + pl.playlistName + "'. You must name them all uniquely. Not adding duplicate named Playlist.");
			}

			return;
		}

        MusicPlaylists.Add(playlist);
    }

    /// <summary>
    /// This method will allow you to delete a Playlist via code.
    /// </summary>
    /// <param name="Playlist">The playlist with all settings included</param>
    public static void DeletePlaylist(string playlistName) {
        var pl = GrabPlaylist(playlistName);

        if (pl == null) {
            return;
        }

        for (var i = 0; i < PlaylistController.Instances.Count; i++) {
            var pc = PlaylistController.Instances[i];
            if (pc.PlaylistName == playlistName) {
                pc.StopPlaylist(false);
                break;
            }
        }

        MusicPlaylists.Remove(pl);
    }

    /// <summary>
    /// This method will allow you to add a song to a Playlist by code.
    /// </summary>
    /// <param name="playlistName">The name of the Playlist to add the song to.</param>
    /// <param name="song">The Audio clip of the song.</param>
    /// <param name="loopSong">Optional - whether or not to loop the song.</param>
    /// <param name="songPitch">Optional - the pitch of the song.</param>
    /// <param name="songVolume">Optional - The volume of the song.</param>
    public static void AddSongToPlaylist(string playlistName, AudioClip song, bool loopSong = false, float songPitch = 1f, float songVolume = 1f) {
        var pl = GrabPlaylist(playlistName);

        if (pl == null) {
            return;
        }

        var newSong = new MusicSetting() {
            clip = song,
            isExpanded = true,
            isLoop = loopSong,
            pitch = songPitch,
            volume = songVolume
        };

        pl.MusicSettings.Add(newSong);
    }

    /// <summary>
    /// This Property can read and set the Playlist Master Volume. 
    /// </summary>
    public static float PlaylistMasterVolume {
        get {
            return MasterAudio.Instance.masterPlaylistVolume;
        }
        set {
            MasterAudio.Instance.masterPlaylistVolume = value;

            var pcs = PlaylistController.Instances;
            for (var i = 0; i < pcs.Count; i++) {
                pcs[i].UpdateMasterVolume();
            }
        }
    }

    #endregion

    #region Custom Events
    /// <summary>fplay
    /// This method is used by MasterAudio to keep track of enabled CustomEventReceivers automatically. This is called when then CustomEventReceiver prefab is enabled.
    /// </summary>
	/// <param name="receiver">The receiver object interface.</param>
	/// <param name="receiverTrans">The receiver object Transform.</param>
	public static void AddCustomEventReceiver(ICustomEventReceiver receiver, Transform receiverTrans) {
        if (AppIsShuttingDown) {
            return;
        }

        if (eventReceivers.ContainsKey(receiver)) {
            return;
        }

        eventReceivers.Add(receiver, receiverTrans);
    }

    /// <summary>
    /// This method is used by MasterAudio to keep track of enabled CustomEventReceivers automatically. This is called when then CustomEventReceiver prefab is disabled.
    /// </summary>
	/// <param name="receiver">The receiver object interface.</param>
	public static void RemoveCustomEventReceiver(ICustomEventReceiver receiver) {
        if (AppIsShuttingDown) {
            return;
        }

        eventReceivers.Remove(receiver);
    }

	public static List<Transform> ReceiversForEvent(string customEventName) {
		var receivers = new List<Transform>();

        foreach (var receiver in eventReceivers.Keys) {
            if (receiver.SubscribesToEvent(customEventName)) {
                receivers.Add(eventReceivers[receiver]);
            }
        }

        return receivers;
    }

    /// <summary>
    /// This method is used to create a Custom Event at runtime.
    /// </summary>
	/// <param name="customEventName">The name of the custom event.</param>
	/// <param name="eventReceiveMode">The receive mode of the event.</param>
	/// <param name="distanceThreshold">The min or max distance to transmit the event to (optional).</param>
	/// <param name="errorOnDuplicate">Whether or not to log an error if the event already exists.</param>
	public static void CreateCustomEvent(string customEventName, CustomEventReceiveMode eventReceiveMode, float distanceThreshold, bool errorOnDuplicate = true) {
        if (AppIsShuttingDown) {
            return;
        }

        if (MasterAudio.Instance.customEvents.FindAll(delegate(CustomEvent obj) {
            return obj.EventName == customEventName;
        }).Count > 0) {
			if (errorOnDuplicate) {
            	Debug.LogError("You already have a Custom Event named '" + customEventName + "'. No need to add it again.");
			}
            return;
        }

		var newEvent = new CustomEvent(customEventName);
		newEvent.eventReceiveMode = eventReceiveMode;
		newEvent.distanceThreshold = distanceThreshold;

        MasterAudio.Instance.customEvents.Add(newEvent);
    }

    /// <summary>
    /// This method is used to delete a temporary Custom Event at runtime.
    /// </summary>
	/// <param name="customEventName">The name of the custom event.</param>
	public static void DeleteCustomEvent(string customEventName) {
        if (AppIsShuttingDown) {
            return;
        }

        MasterAudio.Instance.customEvents.RemoveAll(delegate(CustomEvent obj) {
            return obj.EventName == customEventName;
        });
    }

	private static CustomEvent GetCustomEventByName(string customEventName) {
		var matches =  Instance.customEvents.FindAll (delegate(CustomEvent obj) {
			return obj.EventName == customEventName;
		});
		
		return matches.Count > 0 ? matches[0] : null;
	}

    /// <summary>
	/// Calling this method will fire a Custom Event at the originPoint position. All CustomEventReceivers with the named event specified will do whatever action is assigned to them. If there is a distance criteria applied to receivers, it will be applied.
	/// </summary>
	/// <param name="customEventName">The name of the custom event.</param>
	/// <param name="originPoint">The position of the event.</param>
	public static void FireCustomEvent(string customEventName, Vector3 originPoint) {
		if (AppIsShuttingDown) {
			return;
		}
		 
        if (!CustomEventExists(customEventName) && !IsWarming) {
            Debug.LogError("Custom Event '" + customEventName + "' was not found in Master Audio.");
            return;
        }
		
		var customEvent = GetCustomEventByName(customEventName);

		if (customEvent == null) { // for warming
			return;
		}

		if (!Instance.disableLogging && Instance.logCustomEvents) {
			Debug.Log("Firing Custom Event: " + customEventName);
		}

		float? sqrDist = null; 
		switch (customEvent.eventReceiveMode) {
			case CustomEventReceiveMode.Never:
				if (Instance.LogSounds) {
					Debug.LogWarning("Custom Event '" + customEventName + "' not being transmitted because it is set to 'Never transmit'.");
				}
				return; // no transmission.
			case CustomEventReceiveMode.WhenDistanceLessThan:
			case CustomEventReceiveMode.WhenDistanceMoreThan:
				sqrDist = customEvent.distanceThreshold * customEvent.distanceThreshold;
				break;
		}
		
		
		foreach (var receiver in eventReceivers.Keys) 		{
			if (!receiver.SubscribesToEvent(customEventName)) {
				continue;
			}
			
			switch (customEvent.eventReceiveMode) {
				case CustomEventReceiveMode.WhenDistanceLessThan:
					var dist = (eventReceivers[receiver].position - originPoint).sqrMagnitude;
					if (dist > sqrDist) {
						continue;
					}
					break;
				case CustomEventReceiveMode.WhenDistanceMoreThan:
					var dist2 = (eventReceivers[receiver].position - originPoint).sqrMagnitude;
					if (dist2 < sqrDist) {
						continue;
					}
					break;
			}
			
			receiver.ReceiveEvent(customEventName, originPoint);
		}
    }

    /// <summary>
    /// Calling this method will return whether or not the specified Custom Event exists.
    /// </summary>
	/// <param name="customEventName">The name of the custom event.</param>
	public static bool CustomEventExists(string customEventName) {
        if (AppIsShuttingDown) {
            return true;
        }

        return MasterAudio.Instance.customEvents.FindAll(delegate(CustomEvent obj) {
            return obj.EventName == customEventName;
        }).Count > 0;
    }

    #endregion

    #region Logging (only when turned on via Inspector)
    private static bool LoggingEnabledForGroup(MasterAudioGroup grp) {
		if (Instance.disableLogging) {
			return false;
		}

		if (grp != null && grp.logSound) {
			return true;
		}

		return Instance.LogSounds;
	}

	private static void LogMessage(string message) {
        Debug.Log("T: " + Time.time + " - MasterAudio " + message);
    }

    /// <summary>
    /// This gets or sets whether Logging is enabled in Master Audio
    /// </summary>
    public static bool LogSoundsEnabled {
        get {
            return MasterAudio.Instance.LogSounds;
        }
        set {
            MasterAudio.Instance.LogSounds = value;
        }
    }

    public static void LogWarning(string msg) {
        if (MasterAudio.Instance.disableLogging) {
            return;
        }

        Debug.LogWarning(msg);
    }

    public static void LogError(string msg) {
        if (MasterAudio.Instance.disableLogging) {
            return;
        }

        Debug.LogError(msg);
    }

    public static void LogNoPlaylist(string playlistControllerName, string methodName) {
        LogWarning("There is currently no Playlist assigned to Playlist Controller '" + playlistControllerName + "'. Cannot call '" + methodName + "' method.");
    }

    private static bool IsOkToCallOnlyPlaylistMethod(List<PlaylistController> pcs, string methodName) {
        if (pcs.Count == 0) {
            LogError(string.Format("You have no Playlist Controllers in the Scene. You cannot '{0}'.", methodName));
            return false;
        } else if (pcs.Count > 1) {
            LogError(string.Format("You cannot call '{0}' without specifying a Playlist Controller name when you have more than one Playlist Controller.", methodName));
            return false;
        }

        return true;
    }
    #endregion

    #region Properties
    public static Transform AudioListenerTransform {
        get {
            if (_audListenerTrans == null) {
                var audListener = (AudioListener)GameObject.FindObjectOfType(typeof(AudioListener));
                _audListenerTrans = audListener.transform;
            }

            return _audListenerTrans;
        }
    }

    public static PlaylistController OnlyPlaylistController {
        get {
            var pcs = PlaylistController.Instances;
            if (pcs.Count == 0) {
                Debug.LogError("There are no Playlist Controller in this Scene.");
                return null;
            }

            return pcs[0];
        }
    }

    public static bool IsWarming {
        get {
            return Instance.warming;
        }
    }

    /// <summary>
    /// This gets or sets whether the entire Mixer is muted or not.
    /// </summary>
    public static bool MixerMuted {
        get {
            return MasterAudio.Instance.mixerMuted;
        }
        set {
            MasterAudio.Instance.mixerMuted = value;

            if (value) {
                foreach (var key in audioSourcesBySoundType.Keys) {
                    MuteGroup(audioSourcesBySoundType[key]._group.GameObjectName);
                }
            } else {
                foreach (var key in audioSourcesBySoundType.Keys) {
                    UnmuteGroup(audioSourcesBySoundType[key]._group.GameObjectName);
                }
            }
        }
    }

    /// <summary>
    /// This gets or sets whether the all Playlists are muted or not.
    /// </summary>
    public static bool PlaylistsMuted {
        get {
            return MasterAudio.Instance.playlistsMuted;
        }
        set {
            MasterAudio.Instance.playlistsMuted = value;

            var pcs = PlaylistController.Instances;

            for (var i = 0; i < pcs.Count; i++) {
                if (value) {
                    pcs[i].MutePlaylist();
                } else {
                    pcs[i].UnmutePlaylist();
                }
            }
        }
    }

    /// <summary>
    /// This gets or sets whether music ducking is enabled.
    /// </summary>
    public bool EnableMusicDucking {
        get {
            return enableMusicDucking;
        }
        set {
            enableMusicDucking = value;
        }
    }

    /// <summary>
    /// This gets or sets the ducked volume multiplier
    /// </summary>
    public float DuckedVolumeMultiplier {
        get {
            return duckedVolumeMultiplier;
        }
        set {
            duckedVolumeMultiplier = value;

            var pcs = PlaylistController.Instances;

            for (var i = 0; i < pcs.Count; i++) {
                pcs[i].UpdateDuckedVolumeMultiplier();
            }
        }
    }

    /// <summary>
    /// This gets the cross-fade time for Playlists
    /// </summary>
    public float MasterCrossFadeTime {
        get {
            return crossFadeTime;
        }
    }

    public static List<Playlist> MusicPlaylists {
        get {
            return MasterAudio.Instance.musicPlaylists;
        }
    }

    /// <summary>
    /// This returns of list of all Buses.
    /// </summary>
    public static List<GroupBus> GroupBuses {
        get {
            return MasterAudio.Instance.groupBuses;
        }
    }

    /// <summary>
    /// This will get you the list of all Sound Group Names at runtime only.
    /// </summary>
    public static List<string> RuntimeSoundGroupNames {
        get {
            if (!Application.isPlaying) {
                return new List<string>();
            }
            return new List<string>(audioSourcesBySoundType.Keys);
        }
    }

    /// <summary>
    /// This will get you the list of all Bus Names at runtime only.
    /// </summary>
    public static List<string> RuntimeBusNames {
        get {
            if (!Application.isPlaying) {
                return new List<string>();
            }

            var busNames = new List<string>();

            for (var i = 0; i < MasterAudio.Instance.groupBuses.Count; i++) {
                busNames.Add(MasterAudio.Instance.groupBuses[i].busName);
            }

            return busNames;
        }
    }

    /// <summary>
    /// This property returns a reference to the Singleton instance of MasterAudio, but does not log anything to the console. This is used by PersistentAudioSettings script only.
    /// </summary>
    public static MasterAudio SafeInstance {
        get {
            return (MasterAudio)GameObject.FindObjectOfType(typeof(MasterAudio));
        }
    }

    /// <summary>
    /// This property returns a reference to the Singleton instance of MasterAudio.
    /// </summary>
    public static MasterAudio Instance {
        get {
            if (_instance == null) {
                _instance = (MasterAudio)GameObject.FindObjectOfType(typeof(MasterAudio));
                if (_instance == null && Application.isPlaying) {
                    Debug.LogError("There is no Master Audio prefab in this Scene. Subsequent method calls will fail.");
                }
            }

            return _instance;
        }
        set {
            _instance = null; // to not cache for Inspectors
        }
    }

    public static AudioSource PreviewerInstance {
        get {
            if (_previewerInstance == null) {
                _previewerInstance = MasterAudio.Instance.GetComponent<AudioSource>();
                if (_previewerInstance == null) {
                    MasterAudio.Instance.gameObject.AddComponent<AudioSource>();
                    _previewerInstance = MasterAudio.Instance.GetComponent<AudioSource>();
                    _previewerInstance.priority = AudioPrioritizer.MAX_PRIORITY;
                }

                _previewerInstance.playOnAwake = false;
            }

            return _previewerInstance;
        }
    }

    /// <summary>
    /// This returns true if MasterAudio is initialized and ready to use, false otherwise.
    /// </summary>
    public static bool SoundsReady {
        get {
            return MasterAudio.Instance != null && MasterAudio.Instance.soundsLoaded;
        }
    }

    /// <summary>
    /// This property is used to prevent bogus Unity errors while the editor is stopping play. You should never need to read or set this.
    /// </summary>
    public static bool AppIsShuttingDown {
        get {
            return appIsShuttingDown;
        }
        set {
            appIsShuttingDown = value;
        }
    }

    /// <summary>
    /// This will return a list of all the Sound Group names.
    /// </summary>
    public List<string> GroupNames {
        get {
            var groupNames = new List<string>();
            groupNames.Add(DYNAMIC_GROUP_NAME);
            groupNames.Add(NO_GROUP_NAME);

			var others = new List<string>(this.Trans.childCount);
			for (var i = 0; i < this.Trans.childCount; i++) {
				others.Add(this.Trans.GetChild(i).name);
            }

			others.Sort();
			groupNames.AddRange(others);

            return groupNames;
        }
    }

    /// <summary>
    /// This will return a list of all the Bus names, including the selectors for "type in" and "no bus".
    /// </summary>
    public List<string> BusNames {
        get {
            var busNames = new List<string>();

            busNames.Add(DYNAMIC_GROUP_NAME);
            busNames.Add(NO_GROUP_NAME);

            for (var i = 0; i < groupBuses.Count; i++) {
                busNames.Add(groupBuses[i].busName);
            }

            return busNames;
        }
    }

    /// <summary>
    /// This will return a list of all the Playlists, including the selectors for "type in" and "no bus".
    /// </summary>
    public List<string> PlaylistNames {
        get {
            var playlistNames = new List<string>();

            playlistNames.Add(DYNAMIC_GROUP_NAME);
            playlistNames.Add(NO_GROUP_NAME);

            for (var i = 0; i < musicPlaylists.Count; i++) {
                playlistNames.Add(musicPlaylists[i].playlistName);
            }

            return playlistNames;
        }
    }

	public Transform Trans {
		get {
			if (trans == null) {
				trans = this.GetComponent<Transform>();
			}

			return trans;
		}
	}

    /// <summary>
    /// This will return a list of all the Custom Events you have defined, including the selectors for "type in" and "none".
    /// </summary>
    public List<string> CustomEventNames {
        get {
            var customEventNames = new List<string>();

            customEventNames.Add(DYNAMIC_GROUP_NAME);
            customEventNames.Add(NO_GROUP_NAME);

            var custEvents = MasterAudio.Instance.customEvents;

            for (var i = 0; i < custEvents.Count; i++) {
                customEventNames.Add(custEvents[i].EventName);
            }

            return customEventNames;
        }
    }

    /// <summary>
    /// This is the overall master volume level which can change the relative volume of all buses and Sound Groups - not Playlist Controller songs though, they have their own master volume.
    /// </summary>
    public static float MasterVolumeLevel {
        get {
            return MasterAudio.Instance.masterAudioVolume;
        }
        set {
            MasterAudio.Instance.masterAudioVolume = value;

            if (!Application.isPlaying) {
                return;
            }

            // change all currently playing sound volumes!
            var sources = audioSourcesBySoundType.GetEnumerator();
            MasterAudioGroup _group = null;
            while (sources.MoveNext()) {
                _group = sources.Current.Value._group;
				SetGroupVolume(_group.GameObjectName, _group.groupMasterVolume); // set to same volume, but it recalcs based on master volume level.
            }
        }
    }

    private static bool SceneHasMasterAudio {
        get {
            return MasterAudio.Instance != null;
        }
    }

    public static bool IgnoreTimeScale {
        get {
            return MasterAudio.Instance.ignoreTimeScale;
        }
    }

    public static YieldInstruction InnerLoopDelay {
        get {
            return innerLoopDelay;
        }
    }

    /// <summary>
    /// This gets or sets the "Dynamic Language" (needs to be set at runtime based on the user's selection) for use with localized Resource Files.
    /// </summary>
    public static SystemLanguage DynamicLanguage {
        get {
            if (!PlayerPrefs.HasKey(STORED_LANGUAGE_NAME_KEY)) {
                PlayerPrefs.SetString(STORED_LANGUAGE_NAME_KEY, SystemLanguage.Unknown.ToString());
            }

            return (SystemLanguage)Enum.Parse(typeof(SystemLanguage), PlayerPrefs.GetString(STORED_LANGUAGE_NAME_KEY));
        }
        set {
            PlayerPrefs.SetString(STORED_LANGUAGE_NAME_KEY, value.ToString());
			AudioResourceOptimizer.ClearSupportLanguageFolder();
        }
    }

	public static bool UseDbScaleForVolume {
		get {
			if (!PlayerPrefs.HasKey(USE_DB_KEY)) {
				PlayerPrefs.SetInt(USE_DB_KEY, 0);
			}

			return PlayerPrefs.GetInt(USE_DB_KEY) > 0;
		}
		set {
			PlayerPrefs.SetInt(USE_DB_KEY, value ? 1 : 0);
		}
	}

	public static float ReprioritizeTime {
		get {
			if (repriTime < 0) {
				repriTime = (Instance.rePrioritizeEverySecIndex + 1) * 0.1f;
			}
			
			return repriTime;
		}
	}

    public static bool HasAsyncResourceLoaderFeature() {
        #if UNITY_4_5_3 || UNITY_4_5_4 || UNITY_4_6
            return Application.HasProLicense();
        #else
            return false;
        #endif
    }

    #endregion

	#region Prefab Creation
	/// <summary>
	/// Creates the master audio prefab in the current Scene.
	/// </summary>
	public static GameObject CreateMasterAudio() {
		var ma = Resources.LoadAssetAtPath(MasterAudioFolderPath + "/Prefabs/MasterAudio.prefab", typeof(GameObject));
		if (ma == null) {
			Debug.LogError("Could not find MasterAudio prefab. Please drag it into the scene yourself. It is located under MasterAudio/Prefabs.");
			return null;
		}
		
		
		var go = GameObject.Instantiate(ma) as GameObject;
		go.name = "MasterAudio";
		return go;
	}
	
	/// <summary>
	/// Creates a Playlist Controller prefab instance in the current Scene.
	/// </summary>
	public static GameObject CreatePlaylistController() {
		var pc = Resources.LoadAssetAtPath(MasterAudioFolderPath + "/Prefabs/PlaylistController.prefab", typeof(GameObject));
		if (pc == null) {
			Debug.LogError("Could not find PlaylistController prefab. Please drag it into the scene yourself. It is located under MasterAudio/Prefabs.");
			return null;
		}
		
		var go = GameObject.Instantiate(pc) as GameObject;
		go.name = "PlaylistController";
		return go;
	}
	
	/// <summary>
	/// Creates a Dynamic Sound Group Creator prefab instance in the current Scene.
	/// </summary>
	public static GameObject CreateDynamicSoundGroupCreator() {
		var pc = Resources.LoadAssetAtPath(MasterAudioFolderPath + "/Prefabs/DynamicSoundGroupCreator.prefab", typeof(GameObject));
		if (pc == null) {
			Debug.LogError("Could not find DynamicSoundGroupCreator prefab. Please drag it into the scene yourself. It is located under MasterAudio/Prefabs.");
			return null;
		}
		var go = GameObject.Instantiate(pc) as GameObject;
		go.name = "DynamicSoundGroupCreator";		
		return go;
	}

	#endregion
}
