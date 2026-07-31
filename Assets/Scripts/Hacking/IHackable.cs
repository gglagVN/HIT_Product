using UnityEngine;

/// <summary>
/// Contract for any object that can react when a hacking puzzle is completed or failed.
/// </summary>
public interface IHackable
{
    void OnHackSuccess();
    void OnHackFailure();
}
