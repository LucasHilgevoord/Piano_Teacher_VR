using MidiPlayerTK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiController : MonoBehaviour
{
    /// <summary>
    /// Midi player which controlls the midi itself
    /// </summary>
    [SerializeField] MidiFilePlayer _midiFilePlayer;

    /// <summary>
    /// Midi player to play sounds in relation with the user's input
    /// </summary>
    [SerializeField] MidiStreamPlayer _midiStreamPlayer;
    [SerializeField, Range(0, 1)] private float _midiVolume = 0.5f;
    [SerializeField] private bool _playOnStart = true;

    #region Callbacks

    /// <summary>
    /// This is fired once the midiPlayer plugin creates a note which can be played/spawned
    /// </summary>
    public event NoteCreated OnNoteCreated;
    public delegate void NoteCreated(MPTKEvent note);
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        _midiStreamPlayer.MPTK_Volume = _midiVolume;
        _midiFilePlayer.MPTK_PlayOnStart = _playOnStart;

        _midiFilePlayer.OnEventStartPlayMidi.AddListener(OnStartPlay);
        _midiFilePlayer.OnEventNotesMidi.AddListener(OnMidiEvent);
        _midiFilePlayer.OnEventEndPlayMidi.AddListener(OnEndPlay);
    }

    private void OnStartPlay(string midiName)
    {
        Debug.Log(midiName);
    }

    /// <summary>
    /// Method will be called by the MIDI sequencer just before the notes
    /// are playing by the MIDI synthesizer (if 'Send To Synth' is enabled)
    /// </summary>
    /// <param name="notes"></param>
    private void OnMidiEvent(List<MPTKEvent> notes)
    {
        //Debug.Log("Creating new note batch: " + notes.Count);

        // Create new notes
        foreach (MPTKEvent note in notes)
        {
            //Debug.Log("Create note! " + note);
            OnNoteCreated?.Invoke(note);
        }
    }

    internal void PlayNote(MPTKEvent note)
    {
        _midiStreamPlayer.MPTK_PlayEvent(note);
    }

    private void OnEndPlay(string midiName, EventEndMidiEnum ev)
    {
        Debug.Log(midiName);
    }
}
