using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SoundGroupVariation))]
public class SoundGroupVariationInspector : Editor {
    private SoundGroupVariation _variation;

    public override void OnInspectorGUI() {
        EditorGUIUtility.LookLikeControls();

        EditorGUI.indentLevel = 0;
        var isDirty = false;

        _variation = (SoundGroupVariation)target;

        if (MasterAudioInspectorResources.logoTexture != null) {
            DTGUIHelper.ShowHeaderTexture(MasterAudioInspectorResources.logoTexture);
        }
		
		var parentGroup = _variation.ParentGroup;
		
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUI.contentColor = Color.green;
        if (GUILayout.Button(new GUIContent("Back to Group", "Select Group in Hierarchy"), EditorStyles.toolbarButton, GUILayout.Width(120))) {
            Selection.activeObject = _variation.transform.parent.gameObject;
        }
        GUILayout.FlexibleSpace();
        GUI.contentColor = Color.white;

        var ma = MasterAudio.Instance;
        var maInScene = ma != null;

        if (maInScene) {
            var buttonPressed = DTGUIHelper.AddVariationButtons();

            switch (buttonPressed) {
                case DTGUIHelper.DTFunctionButtons.Play:
                    if (Application.isPlaying) {
                        MasterAudio.PlaySoundAndForget(_variation.transform.parent.name, 1f, null, 0f, _variation.name);
                    } else {
                        isDirty = true;
                        
						var calcVolume = _variation.VarAudio.volume * parentGroup.groupMasterVolume;	
					
						if (_variation.audLocation == MasterAudio.AudioLocation.ResourceFile) {
                            MasterAudio.PreviewerInstance.Stop();
							var fileName = AudioResourceOptimizer.GetLocalizedFileName(_variation.useLocalization, _variation.resourceFileName);
                            MasterAudio.PreviewerInstance.PlayOneShot(Resources.Load(fileName) as AudioClip, calcVolume);
                        } else {
							_variation.VarAudio.PlayOneShot(_variation.VarAudio.clip, calcVolume);
                        }
                    }
                    break;
                case DTGUIHelper.DTFunctionButtons.Stop:
                    if (Application.isPlaying) {
                        MasterAudio.StopAllOfSound(_variation.transform.parent.name);
                    } else {
                        if (_variation.audLocation == MasterAudio.AudioLocation.ResourceFile) {
                            MasterAudio.PreviewerInstance.Stop();
                        } else {
							_variation.VarAudio.Stop();
                        }
                    }
                    break;
            }
        }

        EditorGUILayout.EndHorizontal();

        if (maInScene && !Application.isPlaying) {
            DTGUIHelper.ShowColorWarning(MasterAudio.PREVIEW_TEXT);
        }

        var oldLocation = _variation.audLocation;
        var newLocation = (MasterAudio.AudioLocation)EditorGUILayout.EnumPopup("Audio Origin", _variation.audLocation);

        if (newLocation != oldLocation) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Audio Origin");
            _variation.audLocation = newLocation;
        }

        switch (_variation.audLocation) {
            case MasterAudio.AudioLocation.Clip:
				var newClip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", _variation.VarAudio.clip, typeof(AudioClip), false);

				if (newClip != _variation.VarAudio.clip) {
					UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation.VarAudio, "assign Audio Clip");
					_variation.VarAudio.clip = newClip;
                }
                break;
            case MasterAudio.AudioLocation.ResourceFile:
                if (oldLocation != _variation.audLocation) {
					if (_variation.VarAudio.clip != null) {
                        Debug.Log("Audio clip removed to prevent unnecessary memory usage on Resource file Variation.");
                    }
					_variation.VarAudio.clip = null;
                }

                EditorGUILayout.BeginVertical();
                var anEvent = Event.current;

                GUI.color = Color.yellow;
                var dragArea = GUILayoutUtility.GetRect(0f, 20f, GUILayout.ExpandWidth(true));
                GUI.Box(dragArea, "Drag Resource Audio clip here to use its name!");
                GUI.color = Color.white;

                var newFilename = string.Empty;

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

								var useLocalization = false;
                                newFilename = DTGUIHelper.GetResourcePath(aClip, ref useLocalization);
                                if (string.IsNullOrEmpty(newFilename)) {
                                    newFilename = aClip.name;
                                }

    	                        UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Resource filename");
	                            _variation.resourceFileName = newFilename;
								_variation.useLocalization = useLocalization;
                                break;
                            }
                        }
                        Event.current.Use();
                        break;
                }
                EditorGUILayout.EndVertical();

                newFilename = EditorGUILayout.TextField("Resource Filename", _variation.resourceFileName);
                if (newFilename != _variation.resourceFileName) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Resource filename");
                    _variation.resourceFileName = newFilename;
                }

				EditorGUI.indentLevel = 1;

				var newLocal = EditorGUILayout.Toggle("Use Localized Folder", _variation.useLocalization);
				if (newLocal != _variation.useLocalization) {
					UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "toggle Use Localized Folder");
					_variation.useLocalization = newLocal;
				}

                break;
        }

		EditorGUI.indentLevel = 0;
		var newVolume = DTGUIHelper.DisplayVolumeField(_variation.VarAudio.volume, DTGUIHelper.VolumeFieldType.None, 0f, true);
		if (newVolume != _variation.VarAudio.volume) {
			UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation.VarAudio, "change Volume");
			_variation.VarAudio.volume = newVolume;
        }

		var newPitch = EditorGUILayout.Slider("Pitch", _variation.VarAudio.pitch, -3f, 3f);
		if (newPitch != _variation.VarAudio.pitch) {
			UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation.VarAudio, "change Pitch");
			_variation.VarAudio.pitch = newPitch;
        }

		if (parentGroup.curVariationMode == MasterAudioGroup.VariationMode.LoopedChain) {
			DTGUIHelper.ShowLargeBarAlert("Loop Clip is always OFF for Looped Chain Groups");
		} else {
			var newLoop = EditorGUILayout.Toggle("Loop Clip", _variation.VarAudio.loop);
			if (newLoop != _variation.VarAudio.loop) {
				UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation.VarAudio, "toggle Loop");
				_variation.VarAudio.loop = newLoop;
	        }
		}

        var newWeight = EditorGUILayout.IntSlider("Weight (Instances)", _variation.weight, 0, 100);
        if (newWeight != _variation.weight) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Weight");
            _variation.weight = newWeight;
        }

        var newFxTailTime = EditorGUILayout.Slider("FX Tail Time", _variation.fxTailTime, 0f, 10f);
        if (newFxTailTime != _variation.fxTailTime) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change FX Tail Time");
            _variation.fxTailTime = newFxTailTime;
        }

        var newUseRndPitch = EditorGUILayout.BeginToggleGroup("Use Random Pitch", _variation.useRandomPitch);
        if (newUseRndPitch != _variation.useRandomPitch) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "toggle Use Random Pitch");
            _variation.useRandomPitch = newUseRndPitch;
        }

        if (_variation.useRandomPitch) {
            var newMode = (SoundGroupVariation.RandomPitchMode)EditorGUILayout.EnumPopup("Pitch Compute Mode", _variation.randomPitchMode);
            if (newMode != _variation.randomPitchMode) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Pitch Compute Mode");
                _variation.randomPitchMode = newMode;
            }

            var newPitchMin = EditorGUILayout.Slider("Random Pitch Min", _variation.randomPitchMin, -3f, 3f);
            if (newPitchMin != _variation.randomPitchMin) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Random Pitch Min");
                _variation.randomPitchMin = newPitchMin;
                if (_variation.randomPitchMax <= _variation.randomPitchMin) {
                    _variation.randomPitchMax = _variation.randomPitchMin;
                }
            }

            var newPitchMax = EditorGUILayout.Slider("Random Pitch Max", _variation.randomPitchMax, -3f, 3f);
            if (newPitchMax != _variation.randomPitchMax) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Random Pitch Max");
                _variation.randomPitchMax = newPitchMax;
                if (_variation.randomPitchMin > _variation.randomPitchMax) {
                    _variation.randomPitchMin = _variation.randomPitchMax;
                }
            }
        }

        EditorGUILayout.EndToggleGroup();

        var newUseRndVol = EditorGUILayout.BeginToggleGroup("Use Random Volume", _variation.useRandomVolume);
        if (newUseRndVol != _variation.useRandomVolume) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "toggle Use Random Volume");
            _variation.useRandomVolume = newUseRndVol;
        }

        if (_variation.useRandomVolume) {
            var newMode = (SoundGroupVariation.RandomVolumeMode)EditorGUILayout.EnumPopup("Volume Compute Mode", _variation.randomVolumeMode);
            if (newMode != _variation.randomVolumeMode) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Volume Compute Mode");
                _variation.randomVolumeMode = newMode;
            }

            var volMin = 0f;
            if (_variation.randomVolumeMode == SoundGroupVariation.RandomVolumeMode.AddToClipVolume) {
                volMin = -1f;
            }

			var newVolMin = DTGUIHelper.DisplayVolumeField(_variation.randomVolumeMin, DTGUIHelper.VolumeFieldType.None, volMin, true, "Random Volume Min");
            if (newVolMin != _variation.randomVolumeMin) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Random Volume Min");
                _variation.randomVolumeMin = newVolMin;
                if (_variation.randomVolumeMax <= _variation.randomVolumeMin) {
                    _variation.randomVolumeMax = _variation.randomVolumeMin;
                }
            }

			var newVolMax = DTGUIHelper.DisplayVolumeField(_variation.randomVolumeMax, DTGUIHelper.VolumeFieldType.None, volMin, true, "Random Volume Max");
            if (newVolMax != _variation.randomVolumeMax) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Random Volume Max");
                _variation.randomVolumeMax = newVolMax;
                if (_variation.randomVolumeMin > _variation.randomVolumeMax) {
                    _variation.randomVolumeMin = _variation.randomVolumeMax;
                }
            }
        }

        EditorGUILayout.EndToggleGroup();

        var newSilence = EditorGUILayout.BeginToggleGroup("Use Random Delay", _variation.useIntroSilence);
        if (newSilence != _variation.useIntroSilence) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "toggle Use Random Delay");
            _variation.useIntroSilence = newSilence;
        }

        if (_variation.useIntroSilence) {
            var newSilenceMin = EditorGUILayout.Slider("Delay Min (sec)", _variation.introSilenceMin, 0f, 100f);
            if (newSilenceMin != _variation.introSilenceMin) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Delay Min (sec)");
                _variation.introSilenceMin = newSilenceMin;
                if (_variation.introSilenceMin > _variation.introSilenceMax) {
                    _variation.introSilenceMax = newSilenceMin;
                }
            }

            var newSilenceMax = EditorGUILayout.Slider("Delay Max (sec)", _variation.introSilenceMax, 0f, 100f);
            if (newSilenceMax != _variation.introSilenceMax) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Delay Max (sec)");
                _variation.introSilenceMax = newSilenceMax;
                if (_variation.introSilenceMax < _variation.introSilenceMin) {
                    _variation.introSilenceMin = newSilenceMax;
                }
            }
        }

        EditorGUILayout.EndToggleGroup();

		
		var newStart = EditorGUILayout.BeginToggleGroup("Use Random Start Position", _variation.useRandomStartTime);
		if (newStart != _variation.useRandomStartTime) {
			UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "toggle Use Random Start Position");
			_variation.useRandomStartTime = newStart;
		}
		
		if (_variation.useRandomStartTime) {
			var newMin = EditorGUILayout.Slider("Start Min (%)", _variation.randomStartMinPercent, 0f, 100f);
			if (newMin != _variation.randomStartMinPercent) {
				UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "toggle Start Min (%)");
				_variation.randomStartMinPercent = newMin;
                if (_variation.randomStartMaxPercent <= _variation.randomStartMinPercent) {
                    _variation.randomStartMaxPercent = _variation.randomStartMinPercent;
                }
			}
			
			var newMax = EditorGUILayout.Slider("Start Max (%)", _variation.randomStartMaxPercent, 0f, 100f);
			if (newMax != _variation.randomStartMaxPercent) {
				UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "toggle Start Max (%)");
				_variation.randomStartMaxPercent = newMax;
                if (_variation.randomStartMinPercent > _variation.randomStartMaxPercent) {
                    _variation.randomStartMinPercent = _variation.randomStartMaxPercent;
                }
			}
		}
		
		EditorGUILayout.EndToggleGroup();
		
        var newUseFades = EditorGUILayout.BeginToggleGroup("Use Custom Fading", _variation.useFades);
        if (newUseFades != _variation.useFades) {
            UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "toggle Use Custom Fading");
            _variation.useFades = newUseFades;
        }

        if (_variation.useFades) {
            var newFadeIn = EditorGUILayout.Slider("Fade In Time (sec)", _variation.fadeInTime, 0f, 10f);
            if (newFadeIn != _variation.fadeInTime) {
                UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Fade In Time");
                _variation.fadeInTime = newFadeIn;
            }

			if (_variation.VarAudio.loop) {
                DTGUIHelper.ShowColorWarning("*Looped clips cannot have a custom fade out.");
            } else {
                var newFadeOut = EditorGUILayout.Slider("Fade Out time (sec)", _variation.fadeOutTime, 0f, 10f);
                if (newFadeOut != _variation.fadeOutTime) {
                    UndoHelper.RecordObjectPropertyForUndo(ref isDirty, _variation, "change Fade Out Time");
                    _variation.fadeOutTime = newFadeOut;
                }
            }
        }

        EditorGUILayout.EndToggleGroup();

        var filterList = new List<string>() {
			MasterAudio.NO_GROUP_NAME,
			"Low Pass",
			"High Pass",
			"Distortion",
			"Chorus",
			"Echo",
			"Reverb"
		};

        var newFilterIndex = EditorGUILayout.Popup("Add Filter Effect", 0, filterList.ToArray());
        switch (newFilterIndex) {
            case 1:
                AddFilterComponent(typeof(AudioLowPassFilter));
                break;
            case 2:
                AddFilterComponent(typeof(AudioHighPassFilter));
                break;
            case 3:
                AddFilterComponent(typeof(AudioDistortionFilter));
                break;
            case 4:
                AddFilterComponent(typeof(AudioChorusFilter));
                break;
            case 5:
                AddFilterComponent(typeof(AudioEchoFilter));
                break;
            case 6:
                AddFilterComponent(typeof(AudioReverbFilter));
                break;
        }

        if (GUI.changed || isDirty) {
            EditorUtility.SetDirty(target);
        }

        //DrawDefaultInspector();
    }

    private void AddFilterComponent(Type filterType) {
        _variation.gameObject.AddComponent(filterType);
    }
}