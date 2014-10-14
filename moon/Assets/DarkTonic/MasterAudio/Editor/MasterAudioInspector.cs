using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MasterAudio))]
public class MasterAudioInspector : Editor {
    public const string NEW_BUS_NAME = "[NEW BUS]";
    public const string RENAME_ME_BUS_NAME = "[BUS NAME]";

    private const string NO_MUTE_SOLO_ALLOWED = "You cannot mute or solo this Group because the bus it uses is soloed or muted. Please unmute or unsolo the bus instead.";

    public static readonly Color inactiveClr = new Color(.00f, .77f, .33f);
    public static readonly Color activeClr = new Color(.33f, .99f, .66f);

    private bool isValid = true;
    private MasterAudio sounds;

    public List<MasterAudioGroup> groups = new List<MasterAudioGroup>();

    private List<string> playlistNames = new List<string>();
    private bool isDirty = false;

    private readonly List<float> reevaluatePriorityTimes = new List<float>() {
		.1f,
		.2f,
		.3f,
		.4f,
		.5f,
		.6f,
		.7f,
		.8f,
		.9f,
		1.0f
	};

    public override void OnInspectorGUI() {
        EditorGUIUtility.LookLikeControls();
        EditorGUI.indentLevel = 0;

        sounds = (MasterAudio)target;

        if (MasterAudioInspectorResources.logoTexture != null) {
            DTGUIHelper.ShowHeaderTexture(MasterAudioInspectorResources.logoTexture);
        }

        this.ScanGroups();

        if (!isValid) {
            return;
        }

        isDirty = false;

		var sliderIndicatorChars = 6;
		var sliderWidth = 40;
		
		if (MasterAudio.UseDbScaleForVolume) {
			sliderIndicatorChars = 9;
			sliderWidth = 56;
		}

        var isInProjectView = DTGUIHelper.IsPrefabInProjectView(sounds);

        playlistNames = new List<string>();
        MasterAudio.Playlist pList = null;

        var maxPlaylistNameChars = 11;
        for (var i = 0; i < sounds.musicPlaylists.Count; i++) {
            pList = sounds.musicPlaylists[i];

            playlistNames.Add(pList.playlistName);
            if (pList.playlistName.Length > maxPlaylistNameChars) {
                maxPlaylistNameChars = pList.playlistName.Length;
            }
        }

        var groupNameList = GroupNameList;

        var busFilterList = new List<string>();
        busFilterList.Add(MasterAudio.ALL_BUSES_NAME);
        busFilterList.Add(MasterAudioGroup.NO_BUS);

        var maxChars = 9;
        var busList = new List<string>();
        busList.Add(MasterAudioGroup.NO_BUS);
        busList.Add(NEW_BUS_NAME);

        var busVoiceLimitList = new List<string>();
        busVoiceLimitList.Add(MasterAudio.NO_VOICE_LIMIT_NAME);

        for (var i = 1; i <= 32; i++) {
            busVoiceLimitList.Add(i.ToString());
        }

        GroupBus bus = null;
        for (var i = 0; i < sounds.groupBuses.Count; i++) {
            bus = sounds.groupBuses[i];
            busList.Add(bus.busName);
            busFilterList.Add(bus.busName);

            if (bus.busName.Length > maxChars) {
                maxChars = bus.busName.Length;
            }
        }
        var busListWidth = 9 * maxChars;
        var playlistListWidth = 9 * maxPlaylistNameChars;

        PlaylistController.Instances = null;
        var pcs = PlaylistController.Instances;
        var plControllerInScene = pcs.Count > 0;

        // mixer master volume!
        EditorGUILayout.BeginHorizontal();
        var volumeBefore = sounds.masterAudioVolume;
        GUILayout.Label(DTGUIHelper.LabelVolumeField("Master Mixer Volume"));
        GUILayout.Space(14);
		 
		var newMasterVol = DTGUIHelper.DisplayVolumeField(sounds.masterAudioVolume, DTGUIHelper.VolumeFieldType.None);
        if (newMasterVol != sounds.masterAudioVolume) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Master Mixer Volume");
            if (Application.isPlaying) {
                MasterAudio.MasterVolumeLevel = newMasterVol;
            } else {
                sounds.masterAudioVolume = newMasterVol;
            }
        }
        GUILayout.Space(11);

        var mixerMuteButtonPressed = DTGUIHelper.AddMixerMuteButton("Mixer", sounds);

        GUILayout.FlexibleSpace();

        if (mixerMuteButtonPressed == DTGUIHelper.DTFunctionButtons.Mute) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Mixer Mute");

            sounds.mixerMuted = !sounds.mixerMuted;
            if (Application.isPlaying) {
                MasterAudio.MixerMuted = sounds.mixerMuted;
            } else {
                for (var i = 0; i < groups.Count; i++) {
                    var aGroup = groups[i];
                    aGroup.isMuted = sounds.mixerMuted;
                    if (aGroup.isMuted) {
                        aGroup.isSoloed = false;
                    }
                }
            }
        }

        EditorGUILayout.EndHorizontal();

        if (volumeBefore != sounds.masterAudioVolume) {
            // fix it for realtime adjustments!
            MasterAudio.MasterVolumeLevel = sounds.masterAudioVolume;
        }

        // playlist master volume!
        if (plControllerInScene) {
            EditorGUILayout.BeginHorizontal();
			GUILayout.Label(DTGUIHelper.LabelVolumeField("Master Playlist Volume"));
            GUILayout.Space(5);
			var newPlaylistVol = DTGUIHelper.DisplayVolumeField(sounds.masterPlaylistVolume, DTGUIHelper.VolumeFieldType.None);
            if (newPlaylistVol != sounds.masterPlaylistVolume) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Master Playlist Volume");
                if (Application.isPlaying) {
                    MasterAudio.PlaylistMasterVolume = newPlaylistVol;
                } else {
                    sounds.masterPlaylistVolume = newPlaylistVol;
                }
            }
            GUILayout.Space(11);
            var playlistMuteButtonPressed = DTGUIHelper.AddPlaylistMuteButton("All Playlists", sounds);
            if (playlistMuteButtonPressed == DTGUIHelper.DTFunctionButtons.Mute) {
                if (Application.isPlaying) {
                    MasterAudio.PlaylistsMuted = !MasterAudio.PlaylistsMuted;
                } else {
                    sounds.playlistsMuted = !sounds.playlistsMuted;

                    for (var i = 0; i < pcs.Count; i++) {
                        if (sounds.playlistsMuted) {
                            pcs[i].MutePlaylist();
                        } else {
                            pcs[i].UnmutePlaylist();
                        }
                    }
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Master Crossfade Time");
            
			var space = 2;
			if (MasterAudio.UseDbScaleForVolume) {
				space = 31;
			}

			GUILayout.Space(space);
            var newCrossTime = EditorGUILayout.Slider(sounds.crossFadeTime, 0f, 10f, GUILayout.Width(252));
            if (newCrossTime != sounds.crossFadeTime) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Master Crossfade Time");
                sounds.crossFadeTime = newCrossTime;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            // jukebox controls
            if (Application.isPlaying) {
                DisplayJukebox();
            }
        }

        // Localization section Start
        EditorGUI.indentLevel = 0;
        GUI.color = sounds.showLocalization ? activeClr : inactiveClr;
        EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);

        var newLoc = EditorGUILayout.Toggle("Show Languages", sounds.showLocalization);
        if (newLoc != sounds.showLocalization) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Languages");
            sounds.showLocalization = newLoc;
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;

        if (sounds.showLocalization) {
            if (Application.isPlaying) {
                DTGUIHelper.ShowColorWarning("*Language settings cannot be changed during runtime");
            } else {
                int? langToRemove = null;
                int? langToAdd = null;

                for (var i = 0; i < sounds.supportedLanguages.Count; i++) {
                    var aLang = sounds.supportedLanguages[i];
                    EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);

                    EditorGUILayout.LabelField("Supported Lang. " + (i + 1), GUILayout.Width(142));
                    var newLang = (SystemLanguage)EditorGUILayout.EnumPopup("", aLang);
                    if (newLang != aLang) {
                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Supported Language");
                        sounds.supportedLanguages[i] = newLang;
                    }
                    GUILayout.FlexibleSpace();

                    var buttonPressed = DTGUIHelper.AddFoldOutListItemButtons(i, sounds.supportedLanguages.Count, "Supported Language", true, false, false);

                    switch (buttonPressed) {
                        case DTGUIHelper.DTFunctionButtons.Remove:
                            langToRemove = i;
                            break;
                        case DTGUIHelper.DTFunctionButtons.Add:
                            langToAdd = i;
                            break;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (langToAdd.HasValue) {
                    sounds.supportedLanguages.Insert(langToAdd.Value + 1, SystemLanguage.Unknown);
                } else if (langToRemove.HasValue) {
                    if (sounds.supportedLanguages.Count <= 1) {
                        DTGUIHelper.ShowAlert("You cannot delete the last Supported Language, although you do not have to use Localization.");
                    } else {
                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "Delete Supported Language");
                        sounds.supportedLanguages.RemoveAt(langToRemove.Value);
                    }
                }

                if (!sounds.supportedLanguages.Contains(sounds.defaultLanguage)) {
                    DTGUIHelper.ShowLargeBarAlert("Please add your default language under Supported Languages as well.");
                }

                var newLang2 = (SystemLanguage)EditorGUILayout.EnumPopup(new GUIContent("Default Language", "This language will be used if the user's current language is not supported."), sounds.defaultLanguage);
                if (newLang2 != sounds.defaultLanguage) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Default Language");
                    sounds.defaultLanguage = newLang2;
                }

                var newMode = (MasterAudio.LanguageMode)EditorGUILayout.EnumPopup("Language Mode", sounds.langMode);
                if (newMode != sounds.langMode) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Language Mode");
                    sounds.langMode = newMode;
                    AudioResourceOptimizer.ClearSupportLanguageFolder();
                }

                if (sounds.langMode == MasterAudio.LanguageMode.SpecificLanguage) {
                    var newLang = (SystemLanguage)EditorGUILayout.EnumPopup(new GUIContent("Use Specific Language", "This language will be used instead of your computer's setting. This is useful for testing other languages."), sounds.testLanguage);
                    if (newLang != sounds.testLanguage) {
                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Use Specific");
                        sounds.testLanguage = newLang;
                        AudioResourceOptimizer.ClearSupportLanguageFolder();
                    }

                    if (sounds.supportedLanguages.Contains(sounds.testLanguage)) {
                        DTGUIHelper.ShowLargeBarAlert("Please select your Specific Language under Supported Languages as well.");
                        DTGUIHelper.ShowLargeBarAlert("If you do not, it will use your Default Language instead.");
                    }
                } else if (sounds.langMode == MasterAudio.LanguageMode.DynamicallySet) {
                    DTGUIHelper.ShowLargeBarAlert("Dynamic Language currently set to: " + MasterAudio.DynamicLanguage.ToString());
                }
            }
        }


        // Advanced section Start
        EditorGUI.indentLevel = 0;
        GUI.color = sounds.showAdvancedSettings ? activeClr : inactiveClr;
        EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);

        var newAdv = EditorGUILayout.Toggle("Show Advanced", sounds.showAdvancedSettings);
        if (newAdv != sounds.showAdvancedSettings) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Advanced");
            sounds.showAdvancedSettings = newAdv;
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;

        if (sounds.showAdvancedSettings) {
            if (!Application.isPlaying) {
                var newPersist = EditorGUILayout.Toggle(new GUIContent("Persist Across Scenes", "Turn this on only if you need music or other sounds to play across Scene changes. If not, create a different Master Audio prefab in each Scene."), sounds.persistBetweenScenes);
                if (newPersist != sounds.persistBetweenScenes) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Persist Across Scenes");
                    sounds.persistBetweenScenes = newPersist;
                }
            }

            if (sounds.persistBetweenScenes && plControllerInScene) {
                DTGUIHelper.ShowColorWarning("*Playlist Controller will also persist between scenes!");
            }

            var newAutoPrioritize = EditorGUILayout.Toggle(new GUIContent("Apply Distance Priority", "Turn this on to have Master Audio automatically assign Priority to all audio, based on distance from the Audio Listener. Playlist Controller and 2D sounds are unaffected."), sounds.prioritizeOnDistance);
            if (newAutoPrioritize != sounds.prioritizeOnDistance) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Prioritize By Distance");
                sounds.prioritizeOnDistance = newAutoPrioritize;
            }

            if (sounds.prioritizeOnDistance) {
                EditorGUI.indentLevel = 1;

                var reevalIndex = sounds.rePrioritizeEverySecIndex;

                var evalTimes = new List<string>();
                for (var i = 0; i < reevaluatePriorityTimes.Count; i++) {
                    evalTimes.Add(reevaluatePriorityTimes[i].ToString() + " seconds");
                }

                var newRepri = EditorGUILayout.Popup("Reprioritize Time Gap", reevalIndex, evalTimes.ToArray());
                if (newRepri != reevalIndex) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Re-evaluate time");
                    sounds.rePrioritizeEverySecIndex = newRepri;
                }

                var newContinual = EditorGUILayout.Toggle("Use Clip Age Priority", sounds.useClipAgePriority);
                if (newContinual != sounds.useClipAgePriority) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Use Clip Age Priority");
                    sounds.useClipAgePriority = newContinual;
                }
            }

            EditorGUI.indentLevel = 0;

            var newIgnore = EditorGUILayout.Toggle(new GUIContent("Ignore Time Scale", "Turn this option on if you find that Master Audio features you are using do not work when Time Scale is set to zero. This does take a bit of extra performance away, so don't use unless necessary."), sounds.ignoreTimeScale);
            if (newIgnore != sounds.ignoreTimeScale) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Ignore Time Scale");
                sounds.ignoreTimeScale = newIgnore;
            }

            EditorGUILayout.Separator();

            var newVisual = EditorGUILayout.BeginToggleGroup("Show Visual Settings", sounds.visualAdvancedExpanded);
            if (newVisual != sounds.visualAdvancedExpanded) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Visual Settings");
                sounds.visualAdvancedExpanded = newVisual;
            }

            if (sounds.visualAdvancedExpanded) {
                EditorGUI.indentLevel = 1;

                var newGiz = EditorGUILayout.Toggle(new GUIContent("Show Variation Gizmos", "Turning this option on will show you where your Variation and Event Sounds components are in the Scene with an 'M' icon."), sounds.showGizmos);
                if (newGiz != sounds.showGizmos) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Variation Gizmos");
                    sounds.showGizmos = newGiz;
                }
            }

            EditorGUILayout.EndToggleGroup();

			EditorGUILayout.Separator();
			EditorGUI.indentLevel = 0;
			var exp = EditorGUILayout.BeginToggleGroup("Show Fading Settings", sounds.showFadingSettings);
			if (exp != sounds.showFadingSettings) {
				UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Fading Settings");
				sounds.showFadingSettings = exp;
			}
			if (sounds.showFadingSettings) {
				EditorGUI.indentLevel = 1;	
				DTGUIHelper.ShowLargeBarAlert("Fading to zero volume on the following will cause their audio to stop (if checked).");

				var newStop = EditorGUILayout.Toggle("Buses", sounds.stopZeroVolumeBuses);
				if (newStop != sounds.stopZeroVolumeBuses) {
					UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Buses Stop");
					sounds.stopZeroVolumeBuses = newStop;
				}

				newStop = EditorGUILayout.Toggle("Variations", sounds.stopZeroVolumeVariations);
				if (newStop != sounds.stopZeroVolumeVariations) {
					UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Variations Stop");
					sounds.stopZeroVolumeVariations = newStop;
				}

				newStop = EditorGUILayout.Toggle("Sound Groups", sounds.stopZeroVolumeGroups);
				if (newStop != sounds.stopZeroVolumeGroups) {
					UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Sound Groups Stop");
					sounds.stopZeroVolumeGroups = newStop;
				}

				newStop = EditorGUILayout.Toggle(new GUIContent("Playlist Controllers", "Automatic crossfading will not trigger stop."), sounds.stopZeroVolumePlaylists);
				if (newStop != sounds.stopZeroVolumePlaylists) {
					UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Playlist Controllers Stop");
					sounds.stopZeroVolumePlaylists = newStop;
				}
			}

			EditorGUILayout.EndToggleGroup();

            EditorGUILayout.Separator();
            EditorGUI.indentLevel = 0;
            var newResource = EditorGUILayout.BeginToggleGroup("Show Resource File Settings", sounds.resourceAdvancedExpanded);
            if (newResource != sounds.resourceAdvancedExpanded) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Resource File Settings");
                sounds.resourceAdvancedExpanded = newResource;
            }

            if (sounds.resourceAdvancedExpanded) {
                EditorGUI.indentLevel = 1;
                if (MasterAudio.HasAsyncResourceLoaderFeature()) {
                    var newAsync = EditorGUILayout.Toggle(new GUIContent("Always Load Resources Async", "Checking this means that you will not have this control per Sound Group or Playlist. All Resource files will be loaded asynchronously."), sounds.resourceClipsAllLoadAsync);
                    if (newAsync != sounds.resourceClipsAllLoadAsync) {
                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Always Load Resources Async");
                        sounds.resourceClipsAllLoadAsync = newAsync;
                    }
                }

                var newResourcePause = EditorGUILayout.Toggle(new GUIContent("Keep Paused Resources", "If you check this box, Resource files will not be automatically unloaded from memory when you pause them. Use at your own risk!"), sounds.resourceClipsPauseDoNotUnload);
                if (newResourcePause != sounds.resourceClipsPauseDoNotUnload) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Keep Paused Resources");
                    sounds.resourceClipsPauseDoNotUnload = newResourcePause;
                }
            }
            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.Separator();

            EditorGUI.indentLevel = 0;
            var newLog = EditorGUILayout.BeginToggleGroup("Show Logging Settings", sounds.logAdvancedExpanded);
            if (newLog != sounds.logAdvancedExpanded) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Logging Settings");
                sounds.logAdvancedExpanded = newLog;
            }

            if (sounds.logAdvancedExpanded) {
                EditorGUI.indentLevel = 1;
                newLog = EditorGUILayout.Toggle("Disable Logging", sounds.disableLogging);
                if (newLog != sounds.disableLogging) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Disable Logging");
                    sounds.disableLogging = newLog;
                }

                if (!sounds.disableLogging) {
                    newLog = EditorGUILayout.Toggle("Log Sounds", sounds.LogSounds);
                    if (newLog != sounds.LogSounds) {
						UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Log Sounds");
						if (Application.isPlaying) {
                            MasterAudio.LogSoundsEnabled = sounds.LogSounds;
                        }
                        sounds.LogSounds = newLog;
                    }

					newLog = EditorGUILayout.Toggle("Log Custom Events", sounds.logCustomEvents);
					if (newLog != sounds.logCustomEvents) {
						UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Log Custom Events");
						sounds.logCustomEvents = newLog;
					}
                } else {
                    DTGUIHelper.ShowLargeBarAlert("Logging is disabled.");
                }
            }

            EditorGUILayout.EndToggleGroup();
        }

        // Music Ducking Start
        EditorGUI.indentLevel = 0;
        GUI.color = sounds.showMusicDucking ? activeClr : inactiveClr;
        EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);

        var newShowDuck = EditorGUILayout.Toggle("Show Music Ducking", sounds.showMusicDucking);
        if (newShowDuck != sounds.showMusicDucking) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Music Ducking");
            sounds.showMusicDucking = newShowDuck;
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;

        if (sounds.showMusicDucking) {
            var newEnableDuck = EditorGUILayout.BeginToggleGroup("Enable Ducking", sounds.EnableMusicDucking);
            if (newEnableDuck != sounds.EnableMusicDucking) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Enable Ducking");
                sounds.EnableMusicDucking = newEnableDuck;
            }

            EditorGUILayout.Separator();

            var newMult = EditorGUILayout.Slider("Ducked Vol Multiplier", sounds.duckedVolumeMultiplier, 0f, 1f);
            if (newMult != sounds.duckedVolumeMultiplier) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Ducked Vol Multiplier");
                sounds.DuckedVolumeMultiplier = newMult;
            }

            var newDefault = EditorGUILayout.Slider("Default Begin Unduck", sounds.defaultRiseVolStart, 0f, 1f);
            if (newDefault != sounds.defaultRiseVolStart) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Default Begin Unduck");
                sounds.defaultRiseVolStart = newDefault;
            }

            GUI.contentColor = Color.green;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);

            if (GUILayout.Button(new GUIContent("Add Duck Group"), EditorStyles.toolbarButton, GUILayout.Width(100))) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "Add Duck Group");
                sounds.musicDuckingSounds.Add(new DuckGroupInfo() {
                    soundType = MasterAudio.NO_GROUP_NAME,
                    riseVolStart = sounds.defaultRiseVolStart
                });
            }

            EditorGUILayout.EndHorizontal();
            GUI.contentColor = Color.white;
            EditorGUILayout.Separator();

            if (sounds.musicDuckingSounds.Count == 0) {
                DTGUIHelper.ShowLargeBarAlert("You currently have no ducking sounds set up.");
            } else {
                int? duckSoundToRemove = null;

                for (var i = 0; i < sounds.musicDuckingSounds.Count; i++) {
                    var duckSound = sounds.musicDuckingSounds[i];
                    var index = groupNameList.IndexOf(duckSound.soundType);
                    if (index == -1) {
                        index = 0;
                    }

                    EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                    var newIndex = EditorGUILayout.Popup(index, groupNameList.ToArray(), GUILayout.MaxWidth(200));
                    if (newIndex >= 0) {
                        if (index != newIndex) {
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Duck Group");
                        }
                        duckSound.soundType = groupNameList[newIndex];
                    }

                    GUI.contentColor = Color.green;
                    GUILayout.TextField("Begin Unduck " + duckSound.riseVolStart.ToString("N2"), 20, EditorStyles.miniLabel);

                    var newUnduck = GUILayout.HorizontalSlider(duckSound.riseVolStart, 0f, 1f, GUILayout.Width(60));
                    if (newUnduck != duckSound.riseVolStart) {
                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Begin Unduck");
                        duckSound.riseVolStart = newUnduck;
                    }
                    GUI.contentColor = Color.white;

                    GUILayout.FlexibleSpace();
                    GUILayout.Space(10);
                    if (DTGUIHelper.AddDeleteIcon("Duck Sound")) {
                        duckSoundToRemove = i;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (duckSoundToRemove.HasValue) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "delete Duck Group");
                    sounds.musicDuckingSounds.RemoveAt(duckSoundToRemove.Value);
                }

            }
            EditorGUILayout.EndToggleGroup();

            EditorGUILayout.Separator();
        }
        // Music Ducking End

        // Sound Groups Start		
        EditorGUILayout.BeginHorizontal();
        EditorGUI.indentLevel = 0;  // Space will handle this for the header

        GUI.color = sounds.areGroupsExpanded ? activeClr : inactiveClr;
        EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
        var newGroupEx = EditorGUILayout.Toggle("Show Group Mixer", sounds.areGroupsExpanded);
        if (newGroupEx != sounds.areGroupsExpanded) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Group Mixer");
            sounds.areGroupsExpanded = newGroupEx;
        }

        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        GameObject groupToDelete = null;

        if (sounds.areGroupsExpanded) {
            EditorGUI.indentLevel = 0;

            var newGroupMode = (MasterAudio.DragGroupMode)EditorGUILayout.EnumPopup("Bulk Creation Mode", sounds.curDragGroupMode);
            if (newGroupMode != sounds.curDragGroupMode) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Bulk Creation Mode");
                sounds.curDragGroupMode = newGroupMode;
            }

            var newBulkMode = (MasterAudio.AudioLocation)EditorGUILayout.EnumPopup("Variation Create Mode", sounds.bulkLocationMode);
            if (newBulkMode != sounds.bulkLocationMode) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Bulk Variation Mode");
                sounds.bulkLocationMode = newBulkMode;
            }

            if (sounds.bulkLocationMode == MasterAudio.AudioLocation.ResourceFile) {
                DTGUIHelper.ShowColorWarning("*Resource mode: make sure to drag from Resource folders only.");
            }

            // create groups start
            EditorGUILayout.BeginVertical();
            var anEvent = Event.current;

            GUI.color = Color.yellow;

            if (isInProjectView) {
                DTGUIHelper.ShowLargeBarAlert("You are in Project View and cannot create or navigate Groups.");
                DTGUIHelper.ShowLargeBarAlert("Pull this prefab into the Scene to create Groups.");
            } else {
                var dragArea = GUILayoutUtility.GetRect(0f, 35f, GUILayout.ExpandWidth(true));
                GUI.Box(dragArea, "Drag Audio clips here to create groups!");

                GUI.color = Color.white;

                switch (anEvent.type) {
                    case EventType.DragUpdated:
                    case EventType.DragPerform:
                        if (!dragArea.Contains(anEvent.mousePosition)) {
                            break;
                        }

                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                        if (anEvent.type == EventType.DragPerform) {
                            DragAndDrop.AcceptDrag();

                            Transform groupTrans = null;

                            foreach (var dragged in DragAndDrop.objectReferences) {
                                var aClip = dragged as AudioClip;
                                if (aClip == null) {
                                    continue;
                                }

                                if (sounds.curDragGroupMode == MasterAudio.DragGroupMode.OneGroupPerClip) {
                                    CreateSoundGroup(aClip);
                                } else {
                                    if (groupTrans == null) { // one group with variations
                                        groupTrans = CreateSoundGroup(aClip);
                                    } else {
                                        CreateVariation(groupTrans, aClip);
                                        // create the variations
                                    }
                                }
                            }
                        }
                        Event.current.Use();
                        break;
                }
            }
            EditorGUILayout.EndVertical();
            // create groups end

            EditorGUILayout.LabelField("Group Control", EditorStyles.miniBoldLabel);

            if (sounds.groupBuses.Count > 0) {
                var newGroupByBus = GUILayout.Toggle(sounds.groupByBus, "Group by Bus");
                if (newGroupByBus != sounds.groupByBus) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Group by Bus");
                    sounds.groupByBus = newGroupByBus;
                }
            }

            var newBusFilterIndex = -1;
            var busFilterActive = false;

            if (sounds.groupBuses.Count > 0) {
                busFilterActive = true;
                var oldBusFilter = busFilterList.IndexOf(sounds.busFilter);
                if (oldBusFilter == -1) {
                    oldBusFilter = 0;
                }

                newBusFilterIndex = EditorGUILayout.Popup("Bus Filter", oldBusFilter, busFilterList.ToArray());

                var newBusFilter = busFilterList[newBusFilterIndex];

                if (sounds.busFilter != newBusFilter) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Bus Filter");
                    sounds.busFilter = newBusFilter;
                }
            }

            if (this.groups.Count > 0) {
                var newUseTextGroupFilter = EditorGUILayout.Toggle("Use Text Group Filter", sounds.useTextGroupFilter);
                if (newUseTextGroupFilter != sounds.useTextGroupFilter) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Use Text Group Filter");
                    sounds.useTextGroupFilter = newUseTextGroupFilter;
                }

                if (sounds.useTextGroupFilter) {
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.Label("Text Group Filter", GUILayout.Width(140));
                    var newTextFilter = GUILayout.TextField(sounds.textGroupFilter, GUILayout.Width(180));
                    if (newTextFilter != sounds.textGroupFilter) {
                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Text Group Filter");
                        sounds.textGroupFilter = newTextFilter;
                    }
                    GUILayout.Space(10);
                    GUI.contentColor = Color.green;
                    if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(70))) {
                        sounds.textGroupFilter = string.Empty;
                    }
                    GUI.contentColor = Color.white;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();
                }
            }

            EditorGUI.indentLevel = 0;
            DTGUIHelper.DTFunctionButtons groupButtonPressed = DTGUIHelper.DTFunctionButtons.None;

            MasterAudioGroup aGroup = null;
            var filteredGroups = new List<MasterAudioGroup>();

            filteredGroups.AddRange(this.groups);

            if (busFilterActive && !string.IsNullOrEmpty(sounds.busFilter)) {
                if (newBusFilterIndex == 0) {
                    // no filter
                } else if (newBusFilterIndex == 1) {
                    filteredGroups.RemoveAll(delegate(MasterAudioGroup obj) {
                        return obj.busIndex != 0;
                    });
                } else {
                    filteredGroups.RemoveAll(delegate(MasterAudioGroup obj) {
                        return obj.busIndex != newBusFilterIndex;
                    });
                }
            }

            if (sounds.useTextGroupFilter) {
                if (!string.IsNullOrEmpty(sounds.textGroupFilter)) {
                    filteredGroups.RemoveAll(delegate(MasterAudioGroup obj) {
                        return !obj.transform.name.ToLower().Contains(sounds.textGroupFilter.ToLower());
                    });
                }
            }

            var totalVoiceCount = 0;

            if (groups.Count == 0) {
                DTGUIHelper.ShowLargeBarAlert("You currently have zero Sound Groups.");
            } else {
                var groupsFiltered = this.groups.Count - filteredGroups.Count;
                if (groupsFiltered > 0) {
                    DTGUIHelper.ShowLargeBarAlert(string.Format("{0} Group(s) filtered out.", groupsFiltered));
                }

                int? busToCreate = null;

                for (var l = 0; l < filteredGroups.Count; l++) {
                    EditorGUI.indentLevel = 0;
                    aGroup = filteredGroups[l];

                    var groupDirty = false;

                    string sType = string.Empty;
                    if (Application.isPlaying) {
                        sType = aGroup.name;
                    }

                    EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                    var groupName = aGroup.name;

                    if (Application.isPlaying) {
                        var groupVoices = aGroup.ActiveVoices;
                        var totalVoices = aGroup.TotalVoices;

                        GUI.color = Color.yellow;
                        if (groupVoices >= totalVoices) {
                            GUI.contentColor = Color.red;
                        }
                        GUILayout.Label(string.Format("[{0}]", groupVoices));
                        GUI.color = Color.white;
                        GUI.contentColor = Color.white;

                        totalVoiceCount += groupVoices;
                    }

                    EditorGUILayout.LabelField(groupName, EditorStyles.label, GUILayout.MinWidth(50));
                    //GUILayout.Space(90);

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));

                    // find bus.
                    var selectedBusIndex = aGroup.busIndex == -1 ? 0 : aGroup.busIndex;

                    GUI.contentColor = Color.white;
                    GUI.color = Color.cyan;

                    var busIndex = EditorGUILayout.Popup("", selectedBusIndex, busList.ToArray(), GUILayout.Width(busListWidth));
                    if (busIndex == -1) {
                        busIndex = 0;
                    }

                    if (aGroup.busIndex != busIndex && busIndex != 1) {
                        UndoHelper.RecordObjectPropertyForUndo(ref groupDirty, aGroup, "change Group Bus");
                    }

                    if (busIndex != 1) { // don't change the index, so undo will work.
                        aGroup.busIndex = busIndex;
                    }

                    GUI.color = Color.white;

                    if (selectedBusIndex != busIndex) {
                        if (busIndex == 1) {
                            busToCreate = l;
                        } else if (busIndex >= MasterAudio.HARD_CODED_BUS_OPTIONS) {
                            GroupBus newBus = sounds.groupBuses[busIndex - MasterAudio.HARD_CODED_BUS_OPTIONS];
                            if (Application.isPlaying) {
                                var statGroup = MasterAudio.GrabGroup(sType);
                                statGroup.busIndex = busIndex;

                                if (newBus.isMuted) {
                                    MasterAudio.MuteGroup(aGroup.name);
                                } else if (newBus.isSoloed) {
                                    MasterAudio.SoloGroup(aGroup.name);
                                }
                            } else {
                                // check if bus soloed or muted.
                                if (newBus.isMuted) {
                                    aGroup.isMuted = true;
                                    aGroup.isSoloed = false;
                                } else if (newBus.isSoloed) {
                                    aGroup.isMuted = false;
                                    aGroup.isSoloed = true;
                                }
                            }
                        }
                    }

                    GUI.contentColor = Color.green;

					GUILayout.TextField(DTGUIHelper.DisplayVolumeNumber(aGroup.groupMasterVolume, sliderIndicatorChars), sliderIndicatorChars, EditorStyles.miniLabel, GUILayout.Width(sliderWidth));

					var newVol = DTGUIHelper.DisplayVolumeField(aGroup.groupMasterVolume, DTGUIHelper.VolumeFieldType.MixerGroup);
                    if (newVol != aGroup.groupMasterVolume) {
                        UndoHelper.RecordObjectPropertyForUndo(ref groupDirty, aGroup, "change Group Volume");
                        aGroup.groupMasterVolume = newVol;
                        if (Application.isPlaying) {
                            MasterAudio.SetGroupVolume(aGroup.name, aGroup.groupMasterVolume);
                        }
                    }

                    GUI.contentColor = Color.white;
                    DTGUIHelper.AddLedSignalLight(sounds, groupName);

                    groupButtonPressed = DTGUIHelper.AddMixerButtons(aGroup, "Group");

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndHorizontal();

                    GroupBus groupBus = null;
                    var groupBusIndex = aGroup.busIndex - MasterAudio.HARD_CODED_BUS_OPTIONS;
                    if (groupBusIndex >= 0 && groupBusIndex < sounds.groupBuses.Count) {
                        groupBus = sounds.groupBuses[groupBusIndex];
                    }

                    switch (groupButtonPressed) {
                        case DTGUIHelper.DTFunctionButtons.Play:
                            if (Application.isPlaying) {
                                MasterAudio.PlaySoundAndForget(aGroup.name);
                            } else {
                                var rndIndex = UnityEngine.Random.Range(0, aGroup.groupVariations.Count);
                                var rndVar = aGroup.groupVariations[rndIndex];

								var calcVolume = aGroup.groupMasterVolume * rndVar.VarAudio.volume;

                                if (rndVar.audLocation == MasterAudio.AudioLocation.ResourceFile) {
                                    MasterAudio.PreviewerInstance.Stop();
                                    var fileName = AudioResourceOptimizer.GetLocalizedFileName(rndVar.useLocalization, rndVar.resourceFileName);
                                    MasterAudio.PreviewerInstance.PlayOneShot(Resources.Load(fileName) as AudioClip, calcVolume);
                                } else {
									rndVar.VarAudio.PlayOneShot(rndVar.VarAudio.clip, calcVolume);
                                }

                                isDirty = true;
                            }
                            break;
                        case DTGUIHelper.DTFunctionButtons.Stop:
                            if (Application.isPlaying) {
                                MasterAudio.StopAllOfSound(aGroup.name);
                            } else {
                                var hasResourceFile = false;
                                for (var i = 0; i < aGroup.groupVariations.Count; i++) {
									aGroup.groupVariations[i].VarAudio.Stop();
                                    if (aGroup.groupVariations[i].audLocation == MasterAudio.AudioLocation.ResourceFile) {
                                        hasResourceFile = true;
                                    }
                                }

                                if (hasResourceFile) {
                                    MasterAudio.PreviewerInstance.Stop();
                                }
                            }
                            break;
                        case DTGUIHelper.DTFunctionButtons.Mute:
                            if (groupBus != null && (groupBus.isMuted || groupBus.isSoloed)) {
                                if (Application.isPlaying) {
                                    Debug.LogWarning(NO_MUTE_SOLO_ALLOWED);
                                } else {
                                    DTGUIHelper.ShowAlert(NO_MUTE_SOLO_ALLOWED);
                                }
                            } else {
                                UndoHelper.RecordObjectPropertyForUndo(ref groupDirty, aGroup, "toggle Group mute");

                                if (Application.isPlaying) {
                                    if (aGroup.isMuted) {
                                        MasterAudio.UnmuteGroup(sType);
                                    } else {
                                        MasterAudio.MuteGroup(sType);
                                    }
                                } else {
                                    aGroup.isMuted = !aGroup.isMuted;
                                    if (aGroup.isMuted) {
                                        aGroup.isSoloed = false;
                                    }
                                }
                            }
                            break;
                        case DTGUIHelper.DTFunctionButtons.Solo:
                            if (groupBus != null && (groupBus.isMuted || groupBus.isSoloed)) {
                                if (Application.isPlaying) {
                                    Debug.LogWarning(NO_MUTE_SOLO_ALLOWED);
                                } else {
                                    DTGUIHelper.ShowAlert(NO_MUTE_SOLO_ALLOWED);
                                }
                            } else {
                                UndoHelper.RecordObjectPropertyForUndo(ref groupDirty, aGroup, "toggle Group solo");

                                if (Application.isPlaying) {
                                    if (aGroup.isSoloed) {
                                        MasterAudio.UnsoloGroup(sType);
                                    } else {
                                        MasterAudio.SoloGroup(sType);
                                    }
                                } else {
                                    aGroup.isSoloed = !aGroup.isSoloed;
                                    if (aGroup.isSoloed) {
                                        aGroup.isMuted = false;
                                    }
                                }
                            }
                            break;
                        case DTGUIHelper.DTFunctionButtons.Go:
                            Selection.activeObject = aGroup.transform;
                            break;
                        case DTGUIHelper.DTFunctionButtons.Remove:
                            groupToDelete = aGroup.transform.gameObject;
                            break;
                    }

                    if (groupDirty) {
                        EditorUtility.SetDirty(aGroup);
                    }
                }

                if (busToCreate.HasValue) {
                    CreateBus(busToCreate.Value);
                }

                if (groupToDelete != null) {
                    sounds.musicDuckingSounds.RemoveAll(delegate(DuckGroupInfo obj) {
                        return obj.soundType == groupToDelete.name;
                    });
                    UndoHelper.DestroyForUndo(groupToDelete);
                }

                if (Application.isPlaying) {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(6);
                    GUI.color = Color.yellow;
                    EditorGUILayout.LabelField(string.Format("[{0}] Total Active Voices", totalVoiceCount));
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUI.contentColor = Color.green;
                if (GUILayout.Button(new GUIContent("Mute/Solo Reset", "Turn off all group mute and solo switches"), EditorStyles.toolbarButton, GUILayout.Width(120))) {
                    UndoHelper.RecordObjectsForUndo(this.groups.ToArray(), "Mute/Solo Reset");

                    for (var l = 0; l < this.groups.Count; l++) {
                        aGroup = this.groups[l];
                        aGroup.isSoloed = false;
                        aGroup.isMuted = false;
                    }

                }

                GUILayout.Space(6);

                if (GUILayout.Button(new GUIContent("Max Group Volumes", "Reset all group volumes to full"), EditorStyles.toolbarButton, GUILayout.Width(120))) {
                    UndoHelper.RecordObjectsForUndo(this.groups.ToArray(), "Max Group Volumes");

                    for (var l = 0; l < this.groups.Count; l++) {
                        aGroup = this.groups[l];
                        aGroup.groupMasterVolume = 1f;
                    }
                }

                GUI.contentColor = Color.white;

                EditorGUILayout.EndHorizontal();
            }
            // Sound Groups End

            // Buses
            if (sounds.groupBuses.Count > 0) {
                EditorGUILayout.Separator();
                var oneVoiceBuses = sounds.groupBuses.FindAll(delegate(GroupBus obj) {
                    return obj.voiceLimit == 1;
                }).Count;

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bus Control", EditorStyles.miniBoldLabel, GUILayout.Width(100));
                if (oneVoiceBuses > 0) {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Dialog Mode", EditorStyles.miniBoldLabel, GUILayout.Width(100));
                    GUILayout.Space(276);
                }
                EditorGUILayout.EndHorizontal();

                GroupBus aBus = null;
                DTGUIHelper.DTFunctionButtons busButtonPressed = DTGUIHelper.DTFunctionButtons.None;
                int? busToDelete = null;
                int? busToSolo = null;
                int? busToMute = null;
                int? busToStop = null;

                for (var i = 0; i < sounds.groupBuses.Count; i++) {
                    aBus = sounds.groupBuses[i];

                    EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
                    GUI.color = Color.gray;

                    if (Application.isPlaying) {
                        GUI.color = Color.yellow;
                        if (aBus.BusVoiceLimitReached) {
                            GUI.contentColor = Color.red;
                        }
                        GUILayout.Label(string.Format("[{0:D2}]", aBus.ActiveVoices));
                        GUI.color = Color.white;
                        GUI.contentColor = Color.white;
                    }

                    GUI.color = Color.white;
                    var newBusName = EditorGUILayout.TextField("", aBus.busName, GUILayout.MaxWidth(170));
                    if (newBusName != aBus.busName) {
                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Bus Name");
                        aBus.busName = newBusName;
                    }

                    GUILayout.FlexibleSpace();

                    if (aBus.voiceLimit == 1) {
                        GUI.color = Color.yellow;
                        var newMono = GUILayout.Toggle(aBus.isMonoBus, new GUIContent("", "Dialog Bus Mode"));
                        if (newMono != aBus.isMonoBus) {
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Dialog Bus Mode");
                            aBus.isMonoBus = newMono;
                        }
                    }

                    GUI.color = Color.white;
                    GUILayout.Label("Voices");

                    GUI.color = Color.cyan;
                    var oldLimitIndex = busVoiceLimitList.IndexOf(aBus.voiceLimit.ToString());
                    if (oldLimitIndex == -1) {
                        oldLimitIndex = 0;
                    }
                    var busVoiceLimitIndex = EditorGUILayout.Popup("", oldLimitIndex, busVoiceLimitList.ToArray(), GUILayout.MaxWidth(70));
                    if (busVoiceLimitIndex != oldLimitIndex) {
                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Bus Voice Limit");
                        aBus.voiceLimit = busVoiceLimitIndex <= 0 ? -1 : busVoiceLimitIndex;
                    }

                    GUI.color = Color.white;

                    EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
                    GUI.color = Color.green;
					GUILayout.TextField(DTGUIHelper.DisplayVolumeNumber(aBus.volume, sliderIndicatorChars), sliderIndicatorChars, EditorStyles.miniLabel, GUILayout.Width(sliderWidth));

                    GUI.color = Color.white;
					var newBusVol =DTGUIHelper.DisplayVolumeField(aBus.volume, DTGUIHelper.VolumeFieldType.Bus);
                    if (newBusVol != aBus.volume) {
                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Bus Volume");
                        aBus.volume = newBusVol;
                        if (Application.isPlaying) {
                            MasterAudio.SetBusVolumeByName(aBus.busName, aBus.volume);
                        }
                    }

                    GUI.contentColor = Color.white;

                    busButtonPressed = DTGUIHelper.AddMixerBusButtons(aBus);

                    switch (busButtonPressed) {
                        case DTGUIHelper.DTFunctionButtons.Remove:
                            busToDelete = i;
                            break;
                        case DTGUIHelper.DTFunctionButtons.Solo:
                            busToSolo = i;
                            break;
                        case DTGUIHelper.DTFunctionButtons.Mute:
                            busToMute = i;
                            break;
                        case DTGUIHelper.DTFunctionButtons.Stop:
                            busToStop = i;
                            break;
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndHorizontal();
                }

                if (busToDelete.HasValue) {
                    DeleteBus(busToDelete.Value);
                }
                if (busToMute.HasValue) {
                    MuteBus(busToMute.Value);
                }
                if (busToSolo.HasValue) {
                    SoloBus(busToSolo.Value);
                }
                if (busToStop.HasValue) {
                    MasterAudio.StopBus(sounds.groupBuses[busToStop.Value].busName);
                }

                if (Application.isPlaying) {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(6);
                    GUI.color = Color.yellow;
                    EditorGUILayout.LabelField(string.Format("[{0:D2}] Total Active Voices", totalVoiceCount));
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUI.contentColor = Color.green;

                if (GUILayout.Button(new GUIContent("Mute/Solo Reset", "Turn off all bus mute and solo switches"), EditorStyles.toolbarButton, GUILayout.Width(120))) {
                    BusMuteSoloReset();
                }

                GUILayout.Space(6);

                if (GUILayout.Button(new GUIContent("Max Bus Volumes", "Reset all bus volumes to full"), EditorStyles.toolbarButton, GUILayout.Width(120))) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "Max Bus Volumes");

                    for (var l = 0; l < sounds.groupBuses.Count; l++) {
                        aBus = sounds.groupBuses[l];
                        aBus.volume = 1f;
                    }
                }

                GUI.contentColor = Color.white;

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Separator();
        }
        // Sound Buses End


        // Music playlist Start		
        EditorGUILayout.BeginHorizontal();
        EditorGUI.indentLevel = 0;  // Space will handle this for the header

        GUI.color = sounds.playListExpanded ? activeClr : inactiveClr;
        EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
        var isExp = EditorGUILayout.Toggle("Show Playlist Settings", sounds.playListExpanded);
        if (isExp != sounds.playListExpanded) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Playlist Settings");
            sounds.playListExpanded = isExp;
        }

        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;

        EditorGUILayout.EndHorizontal();

        if (sounds.playListExpanded) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Playlist Controller Setup", EditorStyles.miniBoldLabel, GUILayout.Width(146));
            if (plControllerInScene) {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Sync Grp.", EditorStyles.miniBoldLabel, GUILayout.Width(54));
                EditorGUILayout.LabelField("Initial Playlist", EditorStyles.miniBoldLabel, GUILayout.Width(100));
                GUILayout.Space(204);
            }
            EditorGUILayout.EndHorizontal();

            if (!plControllerInScene) {
                DTGUIHelper.ShowLargeBarAlert("There are no Playlist Controllers in the scene. Music will not play.");
            } else {
                int? indexToDelete = null;

                playlistNames.Insert(0, MasterAudio.NO_PLAYLIST_NAME);

                var syncGroupList = new List<string>();
                for (var i = 0; i < 4; i++) {
                    syncGroupList.Add((i + 1).ToString());
                }
                syncGroupList.Insert(0, MasterAudio.NO_GROUP_NAME);

                for (var i = 0; i < pcs.Count; i++) {
                    var controller = pcs[i];
                    EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                    GUILayout.Label(controller.name);

                    GUILayout.FlexibleSpace();

                    var ctrlDirty = false;

                    GUI.color = Color.cyan;
                    var syncIndex = syncGroupList.IndexOf(controller.syncGroupNum.ToString());
                    if (syncIndex == -1) {
                        syncIndex = 0;
                    }
                    var newSync = EditorGUILayout.Popup("", syncIndex, syncGroupList.ToArray(), GUILayout.Width(55));
                    if (newSync != syncIndex) {
                        UndoHelper.RecordObjectPropertyForUndo(ref ctrlDirty, controller, "change Controller Sync Group");
                        controller.syncGroupNum = newSync;
                    }

                    var origIndex = playlistNames.IndexOf(controller.startPlaylistName);
                    if (origIndex == -1) {
                        origIndex = 0;
						controller.startPlaylistName = string.Empty;
                    }
                    var newIndex = EditorGUILayout.Popup("", origIndex, playlistNames.ToArray(), GUILayout.Width(playlistListWidth));
                    if (newIndex != origIndex) {
                        UndoHelper.RecordObjectPropertyForUndo(ref ctrlDirty, controller, "change Playlist Controller initial Playlist");
                        controller.startPlaylistName = playlistNames[newIndex];
                    }
                    GUI.color = Color.white;

                    GUI.contentColor = Color.green;
                    GUILayout.TextField(DTGUIHelper.DisplayVolumeNumber(controller.playlistVolume, sliderIndicatorChars), sliderIndicatorChars, EditorStyles.miniLabel, GUILayout.Width(sliderWidth));
					var newVol = DTGUIHelper.DisplayVolumeField(controller.playlistVolume, DTGUIHelper.VolumeFieldType.PlaylistController);

                    if (newVol != controller.playlistVolume) {
                        UndoHelper.RecordObjectPropertyForUndo(ref ctrlDirty, controller, "change Playlist Controller volume");
                        controller.playlistVolume = newVol;
                        controller.UpdateMasterVolume();
                    }

                    GUI.contentColor = Color.white;

                    var buttonPressed = DTGUIHelper.AddPlaylistControllerSetupButtons(controller, "Playlist Controller", false);

                    EditorGUILayout.EndHorizontal();

                    switch (buttonPressed) {
                        case DTGUIHelper.DTFunctionButtons.Go:
                            Selection.activeObject = controller.transform;
                            break;
                        case DTGUIHelper.DTFunctionButtons.Remove:
                            indexToDelete = i;
                            break;
                        case DTGUIHelper.DTFunctionButtons.Mute:
                            controller.ToggleMutePlaylist();
                            break;
                    }

                    if (ctrlDirty) {
                        EditorUtility.SetDirty(controller);
                    }
                }

                if (indexToDelete.HasValue) {
                    UndoHelper.DestroyForUndo(pcs[indexToDelete.Value].gameObject);
                }
            }

            EditorGUILayout.Separator();
            GUI.contentColor = Color.green;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent("Create Playlist Controller"), EditorStyles.toolbarButton, GUILayout.Width(150))) {
                var go = GameObject.Instantiate(sounds.playlistControllerPrefab.gameObject);
                go.name = "PlaylistController";

                UndoHelper.CreateObjectForUndo(go as GameObject, "create Playlist Controller");
            }
            EditorGUILayout.EndHorizontal();
            GUI.contentColor = Color.white;
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Playlist Setup", EditorStyles.miniBoldLabel);
            EditorGUI.indentLevel = 0;  // Space will handle this for the header

            if (sounds.musicPlaylists.Count == 0) {
                DTGUIHelper.ShowLargeBarAlert("You currently have no Playlists set up.");
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            var oldPlayExpanded = DTGUIHelper.Foldout(sounds.playlistEditorExpanded, string.Format("Playlists ({0})", sounds.musicPlaylists.Count));
            if (oldPlayExpanded != sounds.playlistEditorExpanded) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Playlists");
                sounds.playlistEditorExpanded = oldPlayExpanded;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(100));

            bool addPressed = false;

            var buttonText = "Click to add new Playlist at the end";
            // Add button - Process presses later
			GUI.contentColor = Color.green;
            addPressed = GUILayout.Button(new GUIContent("Add", buttonText),
                                               EditorStyles.toolbarButton);
            GUIContent content;
            content = new GUIContent("Collapse", "Click to collapse all");
            var masterCollapse = GUILayout.Button(content, EditorStyles.toolbarButton);

            content = new GUIContent("Expand", "Click to expand all");
            var masterExpand = GUILayout.Button(content, EditorStyles.toolbarButton);
            if (masterExpand) {
                ExpandCollapseAllPlaylists(true);
            }
            if (masterCollapse) {
                ExpandCollapseAllPlaylists(false);
            }
			GUI.contentColor = Color.white;
			EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            if (sounds.playlistEditorExpanded) {
                int? playlistToRemove = null;
                int? playlistToInsertAt = null;
                int? playlistToMoveUp = null;
                int? playlistToMoveDown = null;

                for (var i = 0; i < sounds.musicPlaylists.Count; i++) {
                    EditorGUILayout.Separator();
                    var aList = sounds.musicPlaylists[i];

                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
                    aList.isExpanded = DTGUIHelper.Foldout(aList.isExpanded, "Playlist: " + aList.playlistName);

                    var playlistButtonPressed = DTGUIHelper.AddFoldOutListItemButtons(i, sounds.musicPlaylists.Count, "playlist", false, true);

                    EditorGUILayout.EndHorizontal();

                    if (aList.isExpanded) {
                        EditorGUI.indentLevel = 0;
                        var newPlaylist = EditorGUILayout.TextField("Name", aList.playlistName);
                        if (newPlaylist != aList.playlistName) {
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Name");
                            aList.playlistName = newPlaylist;
                        }

                        var crossfadeMode = (MasterAudio.Playlist.CrossfadeTimeMode)EditorGUILayout.EnumPopup("Crossfade Mode", aList.crossfadeMode);
                        if (crossfadeMode != aList.crossfadeMode) {
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Crossfade Mode");
                            aList.crossfadeMode = crossfadeMode;
                        }
                        if (aList.crossfadeMode == MasterAudio.Playlist.CrossfadeTimeMode.Override) {
                            var newCF = EditorGUILayout.Slider("Crossfade time (sec)", aList.crossFadeTime, 0f, 10f);
                            if (newCF != aList.crossFadeTime) {
                                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Crossfade time (sec)");
                                aList.crossFadeTime = newCF;
                            }
                        }

                        var newFadeIn = EditorGUILayout.Toggle("Fade In First Song", aList.fadeInFirstSong);
                        if (newFadeIn != aList.fadeInFirstSong) {
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Fade In First Song");
                            aList.fadeInFirstSong = newFadeIn;
                        }

                        var newFadeOut = EditorGUILayout.Toggle("Fade Out Last Song", aList.fadeOutLastSong);
                        if (newFadeOut != aList.fadeOutLastSong) {
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Fade Out Last Song");
                            aList.fadeOutLastSong = newFadeOut;
                        }

                        var newTransType = (MasterAudio.SongFadeInPosition)EditorGUILayout.EnumPopup("Song Transition Type", aList.songTransitionType);
                        if (newTransType != aList.songTransitionType) {
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Song Transition Type");
                            aList.songTransitionType = newTransType;
                        }
                        if (aList.songTransitionType == MasterAudio.SongFadeInPosition.SynchronizeClips) {
                            DTGUIHelper.ShowColorWarning("*All clips must be of exactly the same length in this mode.");
                        }

                        EditorGUI.indentLevel = 0;
                        var newBulkMode = (MasterAudio.AudioLocation)EditorGUILayout.EnumPopup("Clip Create Mode", aList.bulkLocationMode);
                        if (newBulkMode != aList.bulkLocationMode) {
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Bulk Clip Mode");
                            aList.bulkLocationMode = newBulkMode;
                        }

                        var playlistHasResource = false;
                        for (var s = 0; s < aList.MusicSettings.Count; s++) {
                            if (aList.MusicSettings[s].audLocation == MasterAudio.AudioLocation.ResourceFile) {
                                playlistHasResource = true;
                                break;
                            }
                        }

                        if (MasterAudio.HasAsyncResourceLoaderFeature() && playlistHasResource) {
                            if (!sounds.resourceClipsAllLoadAsync) {
                                var newAsync = EditorGUILayout.Toggle(new GUIContent("Load Resources Async", "Checking this means Resource files in this Playlist will be loaded asynchronously."), aList.resourceClipsAllLoadAsync);
                                if (newAsync != aList.resourceClipsAllLoadAsync) {
                                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Load Resources Async");
                                    aList.resourceClipsAllLoadAsync = newAsync;
                                }
                            }
                        }

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUI.contentColor = Color.green;
                        if (GUILayout.Button(new GUIContent("Equalize Song Volumes"), EditorStyles.toolbarButton)) {
                            EqualizePlaylistVolumes(aList.MusicSettings);
                        }

                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        GUI.contentColor = Color.white;
                        EditorGUILayout.Separator();

                        EditorGUILayout.BeginVertical();
                        var anEvent = Event.current;

                        GUI.color = Color.yellow;

                        var dragArea = GUILayoutUtility.GetRect(0f, 35f, GUILayout.ExpandWidth(true));
                        GUI.Box(dragArea, "Drag Audio clips here to add to playlist!");

                        GUI.color = Color.white;

                        switch (anEvent.type) {
                            case EventType.DragUpdated:
                            case EventType.DragPerform:
                                if (!dragArea.Contains(anEvent.mousePosition)) {
                                    break;
                                }

                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                                if (anEvent.type == EventType.DragPerform) {
                                    DragAndDrop.AcceptDrag();

                                    foreach (var dragged in DragAndDrop.objectReferences) {
                                        var aClip = dragged as AudioClip;
                                        if (aClip == null) {
                                            continue;
                                        }

                                        AddSongToPlaylist(aList, aClip);
                                    }
                                }
                                Event.current.Use();
                                break;
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUI.indentLevel = 2;

                        int? addIndex = null;
                        int? removeIndex = null;
                        int? moveUpIndex = null;
                        int? moveDownIndex = null;

                        if (aList.MusicSettings.Count == 0) {
                            EditorGUI.indentLevel = 0;
                            DTGUIHelper.ShowLargeBarAlert("You currently have no songs in this Playlist.");
                        }

                        EditorGUI.indentLevel = 2;
                        for (var j = 0; j < aList.MusicSettings.Count; j++) {
                            var aSong = aList.MusicSettings[j];
                            var clipName = "Empty";
                            switch (aSong.audLocation) {
                                case MasterAudio.AudioLocation.Clip:
                                    if (aSong.clip != null) {
                                        clipName = aSong.clip.name;
                                    }
                                    break;
                                case MasterAudio.AudioLocation.ResourceFile:
                                    if (!string.IsNullOrEmpty(aSong.resourceFileName)) {
                                        clipName = aSong.resourceFileName;
                                    }
                                    break;
                            }
                            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                            EditorGUI.indentLevel = 2;

                            if (!string.IsNullOrEmpty(clipName) && string.IsNullOrEmpty(aSong.songName)) {
                                switch (aSong.audLocation) {
                                    case MasterAudio.AudioLocation.Clip:
                                        aSong.songName = clipName;
                                        break;
                                    case MasterAudio.AudioLocation.ResourceFile:
                                        aSong.songName = clipName;
                                        break;
                                }
                            }

                            var newSongExpanded = DTGUIHelper.Foldout(aSong.isExpanded, aSong.songName);
                            if (newSongExpanded != aSong.isExpanded) {
                                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Song expand");
                                aSong.isExpanded = newSongExpanded;
                            }
                            var songButtonPressed = DTGUIHelper.AddFoldOutListItemButtons(j, aList.MusicSettings.Count, "clip", false, true, true);
                            EditorGUILayout.EndHorizontal();

                            if (aSong.isExpanded) {
                                EditorGUI.indentLevel = 0;

                                var oldLocation = aSong.audLocation;
                                var newClipSource = (MasterAudio.AudioLocation)EditorGUILayout.EnumPopup("Audio Origin", aSong.audLocation);
                                if (newClipSource != aSong.audLocation) {
                                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Audio Origin");
                                    aSong.audLocation = newClipSource;
                                }

                                switch (aSong.audLocation) {
                                    case MasterAudio.AudioLocation.Clip:
                                        var newClip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", aSong.clip, typeof(AudioClip), true);
                                        if (newClip != aSong.clip) {
                                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Clip");
                                            aSong.clip = newClip;
                                            var cName = newClip == null ? "Empty" : newClip.name;
                                            aSong.songName = cName;
                                        }
                                        break;
                                    case MasterAudio.AudioLocation.ResourceFile:
                                        if (oldLocation != aSong.audLocation) {
                                            if (aSong.clip != null) {
                                                Debug.Log("Audio clip removed to prevent unnecessary memory usage on Resource file Playlist clip.");
                                            }
                                            aSong.clip = null;
                                            aSong.songName = string.Empty;
                                        }

                                        EditorGUILayout.BeginVertical();
                                        anEvent = Event.current;

                                        GUI.color = Color.yellow;
                                        dragArea = GUILayoutUtility.GetRect(0f, 20f, GUILayout.ExpandWidth(true));
                                        GUI.Box(dragArea, "Drag Resource Audio clip here to use its name!");
                                        GUI.color = Color.white;

                                        switch (anEvent.type) {
                                            case EventType.DragUpdated:
                                            case EventType.DragPerform:
                                                if (!dragArea.Contains(anEvent.mousePosition)) {
                                                    break;
                                                }

                                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                                                if (anEvent.type == EventType.DragPerform) {
                                                    DragAndDrop.AcceptDrag();

                                                    foreach (var dragged in DragAndDrop.objectReferences) {
                                                        var aClip = dragged as AudioClip;
                                                        if (aClip == null) {
                                                            continue;
                                                        }

                                                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Resource Filename");

                                                        var unused = false;
                                                        var resourceFileName = DTGUIHelper.GetResourcePath(aClip, ref unused);
                                                        if (string.IsNullOrEmpty(resourceFileName)) {
                                                            resourceFileName = aClip.name;
                                                        }

                                                        aSong.resourceFileName = resourceFileName;
                                                        aSong.songName = aClip.name;
                                                    }
                                                }
                                                Event.current.Use();
                                                break;
                                        }
                                        EditorGUILayout.EndVertical();

                                        var newFilename = EditorGUILayout.TextField("Resource Filename", aSong.resourceFileName);
                                        if (newFilename != aSong.resourceFileName) {
                                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Resource Filename");
                                            aSong.resourceFileName = newFilename;
                                        }

                                        break;
                                }

								var newVol = DTGUIHelper.DisplayVolumeField(aSong.volume, DTGUIHelper.VolumeFieldType.None, 0f, true);
                                if (newVol != aSong.volume) {
                                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Volume");
                                    aSong.volume = newVol;
                                }

                                var newPitch = EditorGUILayout.Slider("Pitch", aSong.pitch, -3f, 3f);
                                if (newPitch != aSong.pitch) {
                                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Pitch");
                                    aSong.pitch = newPitch;
                                }

                                var newLoop = EditorGUILayout.Toggle("Loop Clip", aSong.isLoop);
                                if (newLoop != aSong.isLoop) {
                                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Loop Clip");
                                    aSong.isLoop = newLoop;
                                }

                                if (aList.songTransitionType == MasterAudio.SongFadeInPosition.NewClipFromBeginning) {
                                    var newStart = EditorGUILayout.FloatField("Start Time (seconds)", aSong.customStartTime, GUILayout.Width(300));
                                    if (newStart < 0) {
                                        newStart = 0f;
                                    }
                                    if (newStart != aSong.customStartTime) {
                                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Start Time (seconds)");
                                        aSong.customStartTime = newStart;
                                    }
                                }
                            }

                            switch (songButtonPressed) {
                                case DTGUIHelper.DTFunctionButtons.Add:
                                    addIndex = j;
                                    break;
                                case DTGUIHelper.DTFunctionButtons.Remove:
                                    removeIndex = j;
                                    break;
                                case DTGUIHelper.DTFunctionButtons.ShiftUp:
                                    moveUpIndex = j;
                                    break;
                                case DTGUIHelper.DTFunctionButtons.ShiftDown:
                                    moveDownIndex = j;
                                    break;
                                case DTGUIHelper.DTFunctionButtons.Play:
                                    MasterAudio.PreviewerInstance.Stop();
                                    MasterAudio.PreviewerInstance.PlayOneShot(aSong.clip, aSong.volume);
                                    break;
                                case DTGUIHelper.DTFunctionButtons.Stop:
                                    MasterAudio.PreviewerInstance.clip = null;
                                    MasterAudio.PreviewerInstance.Stop();
                                    break;
                            }
                        }

                        if (addIndex.HasValue) {
                            var mus = new MusicSetting();
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "add song");
                            aList.MusicSettings.Insert(addIndex.Value + 1, mus);
                        } else if (removeIndex.HasValue) {
                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "delete song");
                            aList.MusicSettings.RemoveAt(removeIndex.Value);
                        } else if (moveUpIndex.HasValue) {
                            var item = aList.MusicSettings[moveUpIndex.Value];

                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "shift up song");

                            aList.MusicSettings.Insert(moveUpIndex.Value - 1, item);
                            aList.MusicSettings.RemoveAt(moveUpIndex.Value + 1);
                        } else if (moveDownIndex.HasValue) {
                            var index = moveDownIndex.Value + 1;
                            var item = aList.MusicSettings[index];

                            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "shift down song");

                            aList.MusicSettings.Insert(index - 1, item);
                            aList.MusicSettings.RemoveAt(index + 1);
                        }
                    }

                    switch (playlistButtonPressed) {
                        case DTGUIHelper.DTFunctionButtons.Remove:
                            playlistToRemove = i;
                            break;
                        case DTGUIHelper.DTFunctionButtons.Add:
                            playlistToInsertAt = i;
                            break;
                        case DTGUIHelper.DTFunctionButtons.ShiftUp:
                            playlistToMoveUp = i;
                            break;
                        case DTGUIHelper.DTFunctionButtons.ShiftDown:
                            playlistToMoveDown = i;
                            break;
                    }
                }

                if (playlistToRemove.HasValue) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "delete Playlist");
                    sounds.musicPlaylists.RemoveAt(playlistToRemove.Value);
                }
                if (playlistToInsertAt.HasValue) {
                    var pl = new MasterAudio.Playlist();
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "add Playlist");
                    sounds.musicPlaylists.Insert(playlistToInsertAt.Value + 1, pl);
                }
                if (playlistToMoveUp.HasValue) {
                    var item = sounds.musicPlaylists[playlistToMoveUp.Value];
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "shift up Playlist");
                    sounds.musicPlaylists.Insert(playlistToMoveUp.Value - 1, item);
                    sounds.musicPlaylists.RemoveAt(playlistToMoveUp.Value + 1);
                }
                if (playlistToMoveDown.HasValue) {
                    var index = playlistToMoveDown.Value + 1;
                    var item = sounds.musicPlaylists[index];

                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "shift down Playlist");

                    sounds.musicPlaylists.Insert(index - 1, item);
                    sounds.musicPlaylists.RemoveAt(index + 1);
                }
            }

            if (addPressed) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "add Playlist");
                sounds.musicPlaylists.Add(new MasterAudio.Playlist());
            }
        }
        // Music playlist End

        // Custom Events Start
        EditorGUI.indentLevel = 0;
        GUI.color = sounds.showCustomEvents ? activeClr : inactiveClr;
        EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
        isExp = EditorGUILayout.Toggle("Show Custom Events", sounds.showCustomEvents);
        if (isExp != sounds.showCustomEvents) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Show Custom Events");
            sounds.showCustomEvents = isExp;
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.white;

        if (sounds.showCustomEvents) {
            var newEvent = EditorGUILayout.TextField("New Event Name", sounds.newEventName);
            if (newEvent != sounds.newEventName) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change New Event Name");
                sounds.newEventName = newEvent;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUI.contentColor = Color.green;
            if (GUILayout.Button("Create New Event", EditorStyles.toolbarButton, GUILayout.Width(100))) {
                CreateCustomEvent(sounds.newEventName);
            }
            GUI.contentColor = Color.white;
            EditorGUILayout.EndHorizontal();

            if (sounds.customEvents.Count == 0) {
                DTGUIHelper.ShowLargeBarAlert("You currently have no custom events.");
            }

            EditorGUILayout.Separator();

            int? customEventToDelete = null;
            int? eventToRename = null;

            for (var i = 0; i < sounds.customEvents.Count; i++) {
				EditorGUI.indentLevel = 0;
				var anEvent = sounds.customEvents[i];
				
				EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
				var exp =  DTGUIHelper.Foldout(anEvent.eventExpanded, anEvent.EventName);
				if (exp != anEvent.eventExpanded) {
					UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle expand Custom Event");
					anEvent.eventExpanded = exp;
				}
				
				GUILayout.FlexibleSpace();
				if (Application.isPlaying) {
					var receivers = MasterAudio.ReceiversForEvent(anEvent.EventName);
					
					GUI.contentColor = Color.green;
					if (receivers.Count > 0) {
						if (GUILayout.Button("Select", EditorStyles.toolbarButton, GUILayout.Width(50))) {
							var matches = new List<GameObject>(receivers.Count);
							
							for (var s = 0; s < receivers.Count; s++) {
								Debug.Log(receivers[s]);
								matches.Add(receivers[s].gameObject);
							}
							Selection.objects = matches.ToArray();
						}
					}
					
					if (GUILayout.Button("Fire!", EditorStyles.toolbarButton, GUILayout.Width(50))) {
						MasterAudio.FireCustomEvent(anEvent.EventName, sounds.transform.position); 
					}
					
					GUI.contentColor = Color.yellow;
					GUILayout.Label(string.Format("Receivers: {0}", receivers.Count));
					GUI.contentColor = Color.white;
				} else {
					var newName = GUILayout.TextField(anEvent.ProspectiveName, GUILayout.Width(170));
					if (newName != anEvent.ProspectiveName) {
						UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Proposed Event Name");
						anEvent.ProspectiveName = newName;
					}
					
					var buttonPressed = DTGUIHelper.AddCustomEventDeleteIcon(true);
					
					switch (buttonPressed) {
						case DTGUIHelper.DTFunctionButtons.Remove:
							customEventToDelete = i;
							break;
						case DTGUIHelper.DTFunctionButtons.Rename:
							eventToRename = i;
							break;
						}
				}
				
				EditorGUILayout.EndHorizontal();
				
				
				if (anEvent.eventExpanded) {
					EditorGUI.indentLevel = 1;
					var rcvMode = (MasterAudio.CustomEventReceiveMode) EditorGUILayout.EnumPopup("Send To Receivers", anEvent.eventReceiveMode);	
					if (rcvMode != anEvent.eventReceiveMode) {
						UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Send To Receivers");
						anEvent.eventReceiveMode = rcvMode;
					}
					
					if (rcvMode == MasterAudio.CustomEventReceiveMode.WhenDistanceLessThan || rcvMode == MasterAudio.CustomEventReceiveMode.WhenDistanceMoreThan) {
						var newDist = EditorGUILayout.Slider("Distance Threshold", anEvent.distanceThreshold, 0f, float.MaxValue);
						if (newDist != anEvent.distanceThreshold) {
							UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "change Distance Threshold");
							anEvent.distanceThreshold = newDist;
						}
					}
					
					EditorGUILayout.Separator();
				}
            }

            if (customEventToDelete.HasValue) {
                sounds.customEvents.RemoveAt(customEventToDelete.Value);
            }
            if (eventToRename.HasValue) {
                RenameEvent(sounds.customEvents[eventToRename.Value]);
            }
        }

        // Custom Events End

        if (GUI.changed || isDirty) {
            EditorUtility.SetDirty(target);
        }

        //DrawDefaultInspector();
    }


    private void AddSongToPlaylist(MasterAudio.Playlist pList, AudioClip aClip) {
        MusicSetting lastClip = null;
        if (pList.MusicSettings.Count > 0) {
            lastClip = pList.MusicSettings[pList.MusicSettings.Count - 1];
        }

        MusicSetting mus;

        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "add Song");

        if (lastClip != null && lastClip.clip == null && pList.bulkLocationMode == MasterAudio.AudioLocation.Clip) {
            mus = lastClip;
            mus.clip = aClip;
        } else if (lastClip != null && string.IsNullOrEmpty(lastClip.resourceFileName) && pList.bulkLocationMode == MasterAudio.AudioLocation.ResourceFile) {
            mus = lastClip;
            var unused = false;
            var resourceFileName = DTGUIHelper.GetResourcePath(aClip, ref unused);
            if (string.IsNullOrEmpty(resourceFileName)) {
                resourceFileName = aClip.name;
            } else {
                mus.resourceFileName = resourceFileName;
            }
        } else {
            mus = new MusicSetting() {
                volume = 1f,
                pitch = 1f,
                isExpanded = true,
                audLocation = pList.bulkLocationMode
            };

            switch (pList.bulkLocationMode) {
                case MasterAudio.AudioLocation.Clip:
                    mus.clip = aClip;
                    if (aClip != null) {
                        mus.songName = aClip.name;
                    }
                    break;
                case MasterAudio.AudioLocation.ResourceFile:
                    var unused = false;
                    var resourceFileName = DTGUIHelper.GetResourcePath(aClip, ref unused);
                    if (string.IsNullOrEmpty(resourceFileName)) {
                        resourceFileName = aClip.name;
                    }

                    mus.clip = null;
                    mus.resourceFileName = resourceFileName;
                    mus.songName = aClip.name;
                    break;
            }

            pList.MusicSettings.Add(mus);
        }
    }

    private void CreateVariation(Transform groupTrans, AudioClip aClip) {
        var resourceFileName = string.Empty;
        var useLocalization = false;

        if (sounds.bulkLocationMode == MasterAudio.AudioLocation.ResourceFile) {
            resourceFileName = DTGUIHelper.GetResourcePath(aClip, ref useLocalization);
            if (string.IsNullOrEmpty(resourceFileName)) {
                resourceFileName = aClip.name;
            }
        }

        GameObject newVariation = (GameObject)GameObject.Instantiate(sounds.soundGroupVariationTemplate.gameObject, groupTrans.position, Quaternion.identity);

        var clipName = UtilStrings.TrimSpace(aClip.name);

        newVariation.name = clipName;
        newVariation.transform.parent = groupTrans;

        var variation = newVariation.GetComponent<SoundGroupVariation>();

        if (sounds.bulkLocationMode == MasterAudio.AudioLocation.ResourceFile) {
            variation.audLocation = MasterAudio.AudioLocation.ResourceFile;
            variation.resourceFileName = resourceFileName;
            variation.useLocalization = useLocalization;
        } else {
			variation.VarAudio.clip = aClip;
        }

        newVariation.transform.name = clipName;
    }

    private Transform CreateSoundGroup(AudioClip aClip) {
        var groupName = aClip.name;

        if (sounds.soundGroupTemplate == null || sounds.soundGroupVariationTemplate == null) {
            DTGUIHelper.ShowAlert("Your MasterAudio prefab has been altered and cannot function properly. Please Revert it before continuing.");
            return null;
        }

        if (sounds.transform.FindChild(groupName) != null) {
            DTGUIHelper.ShowAlert("You already have a Sound Group named '" + groupName + "'. Please rename one of them when finished.");
        }

        var resourceFileName = string.Empty;
        var useLocalization = false;

        if (sounds.bulkLocationMode == MasterAudio.AudioLocation.ResourceFile) {
            resourceFileName = DTGUIHelper.GetResourcePath(aClip, ref useLocalization);
            if (string.IsNullOrEmpty(resourceFileName)) {
                resourceFileName = aClip.name;
            }
        }

        var newGroup = (GameObject)GameObject.Instantiate(sounds.soundGroupTemplate.gameObject, sounds.transform.position, Quaternion.identity);

        var grp = newGroup.GetComponent<MasterAudioGroup>();

        var groupTrans = newGroup.transform;
        groupTrans.name = UtilStrings.TrimSpace(groupName);

        var sName = groupName;
        SoundGroupVariation variation = null;

        GameObject newVariation = (GameObject)GameObject.Instantiate(sounds.soundGroupVariationTemplate.gameObject, groupTrans.position, Quaternion.identity);

        variation = newVariation.GetComponent<SoundGroupVariation>();

        if (sounds.bulkLocationMode == MasterAudio.AudioLocation.ResourceFile) {
            variation.audLocation = MasterAudio.AudioLocation.ResourceFile;
            variation.resourceFileName = resourceFileName;
            variation.useLocalization = useLocalization;
            grp.bulkVariationMode = MasterAudio.AudioLocation.ResourceFile;
        } else {
			variation.VarAudio.clip = aClip;
        }

        newVariation.transform.name = sName;
        newVariation.transform.parent = groupTrans;

        groupTrans.parent = sounds.transform;

        MasterAudioGroupInspector.RescanChildren(grp);

        return groupTrans;
    }

    private void CreateBus(int groupIndex) {
        var sourceGroup = this.groups[groupIndex];

        var affectedObjects = new UnityEngine.Object[] {
			sounds,
			sourceGroup
		};

        UndoHelper.RecordObjectsForUndo(affectedObjects, "create Bus");

        var newBus = new GroupBus() {
            busName = RENAME_ME_BUS_NAME
        };
        sounds.groupBuses.Add(newBus);

        sourceGroup.busIndex = MasterAudio.HARD_CODED_BUS_OPTIONS + sounds.groupBuses.Count - 1;
    }

    private void SoloBus(int busIndex) {
        var bus = sounds.groupBuses[busIndex];

        var willSolo = !bus.isSoloed;

        MasterAudioGroup aGroup = null;
        string sType = string.Empty;

        var affectedGroups = new List<MasterAudioGroup>();

        for (var i = 0; i < this.groups.Count; i++) {
            aGroup = this.groups[i];

            if (aGroup.busIndex != MasterAudio.HARD_CODED_BUS_OPTIONS + busIndex) {
                continue;
            }

            affectedGroups.Add(aGroup);
        }

        var allObjects = new List<UnityEngine.Object>();
        allObjects.Add(sounds);

        foreach (var g in affectedGroups) {
            allObjects.Add(g as UnityEngine.Object);
        }

        UndoHelper.RecordObjectsForUndo(allObjects.ToArray(), "solo Bus");

        //change everything
        bus.isSoloed = willSolo;
        if (bus.isSoloed) {
            bus.isMuted = false;
        }

        foreach (var g in affectedGroups) {
            sType = g.name;

            if (Application.isPlaying) {
                if (willSolo) {
                    MasterAudio.UnsoloGroup(sType);
                } else {
                    MasterAudio.SoloGroup(sType);
                }
            }

            g.isSoloed = willSolo;
            if (willSolo) {
                g.isMuted = false;
            }
        }

    }

    private void MuteBus(int busIndex) {
        var bus = sounds.groupBuses[busIndex];

        var willMute = !bus.isMuted;

        MasterAudioGroup aGroup = null;

        var affectedGroups = new List<MasterAudioGroup>();

        for (var i = 0; i < this.groups.Count; i++) {
            aGroup = this.groups[i];

            if (aGroup.busIndex != MasterAudio.HARD_CODED_BUS_OPTIONS + busIndex) {
                continue;
            }

            affectedGroups.Add(aGroup);
        }

        var allObjects = new List<UnityEngine.Object>();
        allObjects.Add(sounds);
        foreach (var g in affectedGroups) {
            allObjects.Add(g as UnityEngine.Object);
        }

        UndoHelper.RecordObjectsForUndo(allObjects.ToArray(), "mute Bus");

        // change everything
        bus.isMuted = willMute;

        if (bus.isSoloed) {
            bus.isSoloed = false;
        }

        foreach (var g in affectedGroups) {
            if (Application.isPlaying) {
                if (!willMute) {
                    MasterAudio.UnmuteGroup(g.name);
                } else {
                    MasterAudio.MuteGroup(g.name);
                }
            } else {
                g.isMuted = willMute;
                if (bus.isMuted) {
                    g.isSoloed = false;
                }
            }
        }
    }

    private void DeleteBus(int busIndex) {
        MasterAudioGroup aGroup = null;

        var groupsWithBus = new List<MasterAudioGroup>();
        var groupsWithHigherBus = new List<MasterAudioGroup>();

        for (var i = 0; i < this.groups.Count; i++) {
            aGroup = this.groups[i];
            if (aGroup.busIndex == -1) {
                continue;
            }
            if (aGroup.busIndex == busIndex + MasterAudio.HARD_CODED_BUS_OPTIONS) {
                groupsWithBus.Add(aGroup);
            } else if (aGroup.busIndex > busIndex + MasterAudio.HARD_CODED_BUS_OPTIONS) {
                groupsWithHigherBus.Add(aGroup);
            }
        }

        var allObjects = new List<UnityEngine.Object>();
        allObjects.Add(sounds);
        foreach (var g in groupsWithBus) {
            allObjects.Add(g as UnityEngine.Object);
        }

        foreach (var g in groupsWithHigherBus) {
            allObjects.Add(g as UnityEngine.Object);
        }

        UndoHelper.RecordObjectsForUndo(allObjects.ToArray(), "delete Bus");

        // change all
        sounds.groupBuses.RemoveAt(busIndex);

        foreach (var group in groupsWithBus) {
            group.busIndex = -1;
        }

        foreach (var group in groupsWithHigherBus) {
            group.busIndex--;
        }
    }

    private void ExpandCollapseAllPlaylists(bool expand) {
        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "toggle Expand / Collapse Playlists");

        for (var i = 0; i < sounds.musicPlaylists.Count; i++) {
            var aList = sounds.musicPlaylists[i];
            aList.isExpanded = expand;

            for (var j = 0; j < aList.MusicSettings.Count; j++) {
                var aSong = aList.MusicSettings[j];
                aSong.isExpanded = expand;
            }
        }
    }

    private void ScanGroups() {
        this.groups.Clear();

        var names = new List<string>();

        for (var i = 0; i < sounds.transform.childCount; i++) {
            var aChild = sounds.transform.GetChild(i);
            if (names.Contains(aChild.name)) {
                DTGUIHelper.ShowRedError("You have more than one group named '" + aChild.name + "'.");
                DTGUIHelper.ShowRedError("Please rename one of them before continuing.");
                isValid = false;
                return;
            }

            names.Add(aChild.name);
            this.groups.Add(aChild.GetComponent<MasterAudioGroup>());
        }

        if (sounds.groupByBus) {
            this.groups.Sort(delegate(MasterAudioGroup g1, MasterAudioGroup g2) {
                if (g1.busIndex == g2.busIndex) {
                    return g1.name.CompareTo(g2.name);
                } else {
                    return g1.busIndex.CompareTo(g2.busIndex);
                }
            });
        } else {
            this.groups.Sort(delegate(MasterAudioGroup g1, MasterAudioGroup g2) {
                return g1.name.CompareTo(g2.name);
            });
        }
    }

    private List<string> GroupNameList {
        get {
            var groupNames = new List<string>();
            groupNames.Add(MasterAudio.NO_GROUP_NAME);

            for (var i = 0; i < groups.Count; i++) {
                groupNames.Add(groups[i].name);
            }

            return groupNames;
        }
    }

    private void DisplayJukebox() {
        EditorGUILayout.Separator();

        var pcs = PlaylistController.Instances;
        PlaylistController pl = null;

        var songNames = new List<string>();

        for (var i = 0; i < pcs.Count; i++) {
            pl = pcs[i];

            GUI.backgroundColor = Color.white;
            GUI.color = Color.cyan;

            EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);
            EditorGUILayout.BeginHorizontal();

            var playlistIndex = -1;
            if (!string.IsNullOrEmpty(pl.startPlaylistName) && pl.HasPlaylist && pl.CurrentPlaylist != null) {
                playlistIndex = playlistNames.IndexOf(pl.CurrentPlaylist.playlistName);

                songNames.Clear();
                for (var s = 0; s < pl.CurrentPlaylist.MusicSettings.Count; s++) {
                    var aSong = pl.CurrentPlaylist.MusicSettings[s];

                    var songName = string.Empty;

                    switch (aSong.audLocation) {
                        case MasterAudio.AudioLocation.Clip:
                            songName = aSong.clip == null ? string.Empty : aSong.clip.name;
                            break;
                        case MasterAudio.AudioLocation.ResourceFile:
                            songName = aSong.resourceFileName;
                            break;
                    }

                    if (string.IsNullOrEmpty(songName)) {
                        continue;
                    }

                    songNames.Add(songName);
                }
            }

            GUILayout.Label(pl.name);

            GUILayout.FlexibleSpace();

            GUI.color = Color.cyan;

            GUILayout.Label("V " + pl.playlistVolume.ToString("N2"));

            var newVol = GUILayout.HorizontalSlider(pl.playlistVolume, 0f, 1f, GUILayout.Width(100));
            if (newVol != pl.playlistVolume) {
                pl.playlistVolume = newVol;
                pl.UpdateMasterVolume();
            }

            GUI.color = Color.white;
            var muteButtonPressed = DTGUIHelper.AddPlaylistControllerSetupButtons(pl, "Playlist Controller", true);

            if (muteButtonPressed == DTGUIHelper.DTFunctionButtons.Mute) {
                pl.ToggleMutePlaylist();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            GUI.backgroundColor = Color.cyan;
            GUI.color = Color.green;

            EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

            var clip = pl.CurrentPlaylistClip;
            string clipPosition = "";
            AudioSource playingSource = null;
            if (clip != null) {
                playingSource = pl == null ? null : pl.CurrentPlaylistSource;
                if (playingSource != null) {
                    clipPosition = "(-" + (clip.length - playingSource.time).ToString("N2") + " secs)";
                }
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Playlist:");
            GUILayout.Space(30);
            var playlistIndexToStart = EditorGUILayout.Popup(playlistIndex, playlistNames.ToArray(), GUILayout.Width(150));

            if (playlistIndex != playlistIndexToStart) {
                pl.ChangePlaylist(playlistNames[playlistIndexToStart]);
            }
            GUILayout.Label(string.Format("[{0}]", pl.PlaylistState));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Active Clip:");
            GUILayout.Space(9);

            var songIndex = -1;
            if (pl.CurrentPlaylistClip != null) {
                songIndex = songNames.IndexOf(pl.CurrentPlaylistClip.name);
            }
            var newSong = EditorGUILayout.Popup(songIndex, songNames.ToArray(), GUILayout.Width(150));
            if (newSong != songIndex) {
                pl.TriggerPlaylistClip(songNames[newSong]);
            }

            if (!string.IsNullOrEmpty(clipPosition)) {
                GUILayout.Label(clipPosition);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            var fadingClip = pl == null ? null : pl.FadingPlaylistClip;
            var fadingClipName = fadingClip == null ? "[None]" : fadingClip.name;
            string fadingClipPosition = "";
            if (fadingClip != null) {
                var fadingSource = pl == null ? null : pl.FadingSource;
                if (fadingSource != null) {
                    fadingClipPosition = "(-" + (fadingClip.length - fadingSource.time).ToString("N2") + " secs)";
                }
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Fading Clip:");
            GUILayout.Space(7);
            GUILayout.Label(fadingClipName + "  " + fadingClipPosition);
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            GUI.color = Color.cyan;


            DTGUIHelper.JukeboxButtons buttonPressed = DTGUIHelper.JukeboxButtons.None;

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbarButton);
            buttonPressed = DTGUIHelper.AddJukeboxIcons();
            if (playingSource != null) {
                var oldtime = playingSource.time;
                var newTime = EditorGUILayout.Slider("", oldtime, 0f, clip.length);
                if (oldtime != newTime) {
                    playingSource.time = newTime;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Separator();

            switch (buttonPressed) {
                case DTGUIHelper.JukeboxButtons.Stop:
                    pl.StopPlaylist();
                    break;
                case DTGUIHelper.JukeboxButtons.NextSong:
                    pl.PlayNextSong();
                    break;
                case DTGUIHelper.JukeboxButtons.Pause:
                    pl.PausePlaylist();
                    break;
                case DTGUIHelper.JukeboxButtons.Play:
                    if (!pl.ResumePlaylist()) {
                        if (pl.CurrentPlaylist != null) {
                            pl.ChangePlaylist(pl.CurrentPlaylist.playlistName);
                        }
                    }
                    break;
                case DTGUIHelper.JukeboxButtons.RandomSong:
                    pl.PlayRandomSong();
                    break;
            }
        }
    }

    private void BusMuteSoloReset() {
        var allObjects = new List<UnityEngine.Object>();
        allObjects.Add(sounds);
        foreach (var g in groups) {
            allObjects.Add(g as UnityEngine.Object);
        }

        UndoHelper.RecordObjectsForUndo(allObjects.ToArray(), "Mute/Solo Reset");

        //reset everything
        GroupBus aBus = null;

        for (var l = 0; l < sounds.groupBuses.Count; l++) {
            aBus = sounds.groupBuses[l];

            if (Application.isPlaying) {
                MasterAudio.UnsoloBus(aBus.busName);
                MasterAudio.UnmuteBus(aBus.busName);
            } else {
                aBus.isSoloed = false;
                aBus.isMuted = false;
            }
        }

        foreach (var gr in groups) {
            if (Application.isPlaying) {
                MasterAudio.UnsoloGroup(gr.name);
                MasterAudio.UnmuteGroup(gr.name);
            } else {
                gr.isSoloed = false;
                gr.isMuted = false;
            }
        }
    }

    private void EqualizePlaylistVolumes(List<MusicSetting> playlistClips) {
        var clips = new Dictionary<MusicSetting, float>();

        if (playlistClips.Count < 2) {
            DTGUIHelper.ShowAlert("You must have at least 2 clips in a Playlist to use this function.");
            return;
        }

        float lowestVolume = 1f;

        for (var i = 0; i < playlistClips.Count; i++) {
			var setting = playlistClips[i];
            var ac = setting.clip;

			if (setting.audLocation == MasterAudio.AudioLocation.Clip && ac == null) {
				continue;
			}
			
			if (setting.audLocation == MasterAudio.AudioLocation.ResourceFile) {
				ac = Resources.Load(setting.resourceFileName) as AudioClip;
				if (ac == null) {
					Debug.LogError("Song '" + setting.resourceFileName + "' could not be loaded and is being skipped.");
					continue;
				}
			}

            float average = 0f;
            var buffer = new float[ac.samples];

            Debug.Log("Measuring amplitude of '" + ac.name + "'.");

            try {
                ac.GetData(buffer, 0);
            }
            catch {
                Debug.Log("Could not read data from compressed sample. Skipping '" + setting.clip.name + "'.");
                continue;
            }

            for (int c = 0; c < ac.samples; c++) {
                average += Mathf.Pow(buffer[c], 2);
            }

            average = Mathf.Sqrt(1f / (float)ac.samples * average);

			if (average == 0) {
				Debug.LogError("Song '" + setting.songName + "' is being excluded because it's compressed or streaming.");
				continue;
			} 

            if (average < lowestVolume) {
                lowestVolume = average;
            }

            clips.Add(setting, average);
        }

        if (clips.Count < 2) {
            DTGUIHelper.ShowAlert("You must have at least 2 clips in a Playlist to use this function.");
            return;
        }

		UndoHelper.RecordObjectPropertyForUndo(ref isDirty, sounds, "Equalize Song Volumes");

        foreach (var kv in clips) {
            float adjustedVol = lowestVolume / kv.Value;
            //set your volume for each song in your playlist.
            kv.Key.volume = adjustedVol;
        }
    }

    private void CreateCustomEvent(string newEventName) {
        if (sounds.customEvents.FindAll(delegate(CustomEvent obj) {
            return obj.EventName == newEventName;
        }).Count > 0) {
            DTGUIHelper.ShowAlert("You already have a custom event named '" + newEventName + "'. Please choose a different name.");
            return;
        }

        sounds.customEvents.Add(new CustomEvent(newEventName));
    }

    private void RenameEvent(CustomEvent cEvent) {
        var match = sounds.customEvents.FindAll(delegate(CustomEvent obj) {
            return obj.EventName == cEvent.ProspectiveName;
        });

        if (match.Count > 0) {
            DTGUIHelper.ShowAlert("You already have a custom event named '" + cEvent.ProspectiveName + "'. Please choose a different name.");
            return;
        }

        cEvent.EventName = cEvent.ProspectiveName;
    }
}