# Weapon Switch Input and HUD Key Hints

## Goal

Make weapon switching discoverable and convenient by showing each weapon's
keyboard shortcut on its existing HUD icon and supporting mouse-wheel cycling.

## Scope

- Keep the existing `GunHolder.weapons` array as the source of weapon order.
- Keep direct number-key selection for weapon slots.
- Add mouse-wheel selection that skips null, locked, or unavailable weapons.
- Keep the current HUD active/inactive weapon presentation and save/load behavior.
- Do not add a separate weapon-selection panel or change weapon unlock rules.

## Behavior

### Number keys

The number displayed on each weapon icon corresponds to its zero-based array
slot plus one (`weapons[0]` displays `1`). Pressing the corresponding number
selects that slot. Invalid slots and locked weapons remain unavailable.

### Mouse wheel

Scrolling up selects the next available weapon; scrolling down selects the
previous available weapon. Selection wraps around the weapon array and skips
null or non-playable weapons. Scrolling does nothing when no other weapon is
available or while the current weapon is reloading.

### HUD hints

The existing weapon icons receive a configurable text label showing their
number. Labels remain visible for locked slots so the player can learn the
mapping, while the existing locked/unlocked visual state remains unchanged.
The implementation must safely handle missing labels, missing HUD manager
references, and weapon arrays that are empty or contain null entries.

## Implementation

- Extend `GunHolder` with:
  - input handling for mouse-wheel direction;
  - a helper to find the next/previous playable weapon with wraparound;
  - safe selection validation before dereferencing a weapon or HUD manager.
- Extend `HUDManager` with an array of existing/new label references for weapon
  icons and a method to refresh their displayed slot numbers.
- Refresh labels during HUD initialization and when the weapon list is first
  synchronized. Existing `GetCurrentGun`, `GetPrevGun`, and ammo updates remain
  responsible for selection visuals.

## Validation

- Validate all changed scripts through Unity's script validator.
- Refresh Unity and confirm there are no new compile errors.
- Run the existing EditMode test assembly, including the weapon/save-related
  tests if present.
- Manually verify number-key selection, locked-weapon skipping, wheel wraparound,
  reload blocking, and HUD labels in the `MAIN` scene.
