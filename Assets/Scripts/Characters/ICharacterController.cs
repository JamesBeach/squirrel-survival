using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/***
 * All game characters should implement this interface. Use of this interface should simplify LevelController and other
 * ubiquitous game management classes by reducing their need to know about specific kinds of characters and by ensuring
 * that all characters have some minimum level of functionality.
 * 
 * <b>Characters MUST register with the LevelController for these functions to be invoked at the expected times.</b>
***/
public interface ICharacterController {

    /// <summary>
    /// The effect of this method should be to cease reversibly <i>all</i> character activity, especially movement but also
    /// including animations and status changes. <b>This method is not a toggle.</b>
    /// </summary>
    void Pause();

    /// <summary>
    /// The effect of this method should be to resume, if necessary, all character activity as needed for the game to
    /// progress as normal.
    /// </summary>
    void UnPause();

    /// <summary>
    /// This method will be called once when a level or scene becomes playable for the first time, indicating that the
    /// character may begin a change in behavior if necessary. In most cases, this action will be similar to UnPause().
    /// It may or may not be necessary to do anything with this call. Cars, for example, are always going but dogs should
    /// refrain from attacking until the game begins.
    /// </summary>
    void Begin();
}
