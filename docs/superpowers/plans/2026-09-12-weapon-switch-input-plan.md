# Weapon Switch Input and HUD Hints Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add weapon-slot number hints to the existing HUD and support mouse-wheel cycling through unlocked weapons without breaking current number-key, reload, HUD, or save/load behavior.

**Architecture:** `GunHolder` remains the single owner of weapon selection and will expose safe slot validation plus wraparound navigation. `HUDManager` will retain its current active/inactive icon logic and gain optional `TextMeshProUGUI[]` references for slot labels, initialized from the weapon slot order. No new weapon-selection panel or unlock system will be introduced.

**Tech Stack:** Unity C#, `UnityEngine.Input`, TextMeshPro, existing `GunHolder`, `HUDManager`, and Unity EditMode validation/tests.

---

### Task 1: Add focused weapon navigation tests

**Files:**
- Create: `Assets/Tests/Editor/GunHolderTests.cs`
- Modify: `Assets/Scripts/GunHolder.cs`

- [ ] **Step 1: Add tests for slot navigation expectations**

Create EditMode tests that instantiate a `GunHolder`, assign a mix of playable and locked `Gun` components, and verify the navigation helper returns the next/previous playable index with wraparound. Include cases for:

```csharp
Assert.AreEqual(2, holder.FindAdjacentPlayableWeapon(0, 1));
Assert.AreEqual(0, holder.FindAdjacentPlayableWeapon(2, 1));
Assert.AreEqual(2, holder.FindAdjacentPlayableWeapon(0, -1));
Assert.AreEqual(-1, holder.FindAdjacentPlayableWeapon(0, 1)); // no other playable slot
```

Destroy all created objects in teardown so the tests do not leak scene objects.

- [ ] **Step 2: Run the focused test and confirm the expected failure**

Run the Unity EditMode test for `GunHolderTests`. Expected result: the test does not compile or fails because `FindAdjacentPlayableWeapon` does not yet exist.

- [ ] **Step 3: Implement the minimal navigation helper**

In `Assets/Scripts/GunHolder.cs`, add a public/testable helper:

```csharp
public int FindAdjacentPlayableWeapon(int startIndex, int direction)
{
    if (weapons == null || weapons.Length < 2 || direction == 0)
        return -1;

    int normalizedDirection = direction > 0 ? 1 : -1;
    for (int step = 1; step < weapons.Length; step++)
    {
        int candidate = (startIndex + normalizedDirection * step) % weapons.Length;
        if (candidate < 0)
            candidate += weapons.Length;

        Gun gun = weapons[candidate] != null
            ? weapons[candidate].GetComponent<Gun>()
            : null;
        if (gun != null && gun.isPlayable)
            return candidate;
    }

    return -1;
}
```

- [ ] **Step 4: Run the focused test and confirm it passes**

Run the same EditMode test. Expected result: all navigation cases pass.

- [ ] **Step 5: Commit the navigation test and helper**

```bash
git add Assets/Tests/Editor/GunHolderTests.cs Assets/Scripts/GunHolder.cs
git commit -m "test: cover playable weapon navigation"
```

### Task 2: Make GunHolder input and selection safe

**Files:**
- Modify: `Assets/Scripts/GunHolder.cs`

- [ ] **Step 1: Replace hard-coded input with slot iteration**

In `Update()`, iterate over the first nine weapon slots and call `SelectWeapon(i)` when `KeyCode.Alpha1 + i` is pressed. Read `Input.mouseScrollDelta.y`; use positive values for direction `1` and negative values for direction `-1`, then call `FindAdjacentPlayableWeapon(currentWeapon, direction)` and select the returned index only when it is non-negative.

- [ ] **Step 2: Guard initialization and current-weapon access**

Update `Start()` and `SelectWeapon()` so they safely handle:

- `weapons == null` or an empty array;
- null weapon entries;
- an invalid `currentWeapon`;
- a missing `Gun` component;
- a missing `HUDManager.Instance`.

Do not dereference the current weapon or HUD manager until each has been checked. Preserve reload blocking and locked-weapon rejection.

- [ ] **Step 3: Keep HUD state synchronized**

After selecting a valid weapon, deactivate all non-selected non-null weapon objects, update `currentWeapon`, and call `GetCurrentGun`, `GetCurrentAmmo`, and `GetPrevGun` only when `HUDManager.Instance` is available.

- [ ] **Step 4: Run the focused tests and compile validation**

Run `GunHolderTests`, then validate `Assets/Scripts/GunHolder.cs` with Unity. Expected result: no new errors, and number-key/navigation tests pass.

- [ ] **Step 5: Commit the input and safety changes**

```bash
git add Assets/Scripts/GunHolder.cs
git commit -m "feat: add safe wheel weapon switching"
```

### Task 3: Add number labels to the existing weapon HUD

**Files:**
- Modify: `Assets/Scripts/HUDManager.cs`
- Modify: `Assets/Scenes/MAIN.unity`

- [ ] **Step 1: Add optional label references and refresh method**

Add a serialized field near the existing weapon HUD fields:

```csharp
[Tooltip("Optional number labels matching listGunIsActive/listGunIsUnactive slots.")]
public TextMeshProUGUI[] weaponSlotNumberUI;
```

Add a public method that writes `i + 1` to each non-null label, clears labels beyond the weapon count when applicable, and safely handles null arrays:

```csharp
public void RefreshWeaponSlotNumbers(int weaponCount)
{
    if (weaponSlotNumberUI == null)
        return;

    for (int i = 0; i < weaponSlotNumberUI.Length; i++)
    {
        if (weaponSlotNumberUI[i] != null)
            weaponSlotNumberUI[i].text = i < weaponCount ? (i + 1).ToString() : string.Empty;
    }
}
```

- [ ] **Step 2: Refresh labels after HUD initialization**

Ensure the HUD calls the new method after its references are available. The `GunHolder` startup path should also call it when `HUDManager.Instance` exists, passing `weapons.Length`, so labels are synchronized even when script execution order differs.

- [ ] **Step 3: Wire existing MAIN HUD text objects**

Inspect the `MAIN` scene's existing active/inactive weapon icon hierarchy, assign matching number `TextMeshProUGUI` objects to `HUDManager.weaponSlotNumberUI`, and save the scene. Preserve the existing icon arrays and active-state behavior.

- [ ] **Step 4: Validate the HUD script and scene**

Validate `Assets/Scripts/HUDManager.cs`, refresh Unity, and inspect Console for compile errors. Expected result: no new errors and the HUD labels contain `1`, `2`, `3`, etc. in slot order.

- [ ] **Step 5: Commit the HUD label changes**

```bash
git add Assets/Scripts/HUDManager.cs Assets/Scenes/MAIN.unity
git commit -m "feat: show weapon slot numbers in HUD"
```

### Task 4: Run regression validation

**Files:**
- Test: `Assets/Tests/Editor/GunHolderTests.cs`
- Validate: `Assets/Scripts/GunHolder.cs`
- Validate: `Assets/Scripts/HUDManager.cs`

- [ ] **Step 1: Run all existing EditMode tests**

Run the project EditMode test assembly, including `GunHolderTests`. Expected result: all discovered tests pass with no failures.

- [ ] **Step 2: Check Unity Console**

Read Unity errors after compilation and test execution. Expected result: no new errors attributable to weapon switching; unrelated pre-existing warnings may remain.

- [ ] **Step 3: Verify behavior in MAIN**

In Play Mode, verify:

1. Each HUD weapon icon displays its slot number.
2. Number keys select the matching unlocked weapon.
3. Mouse-wheel up/down cycles forward/backward.
4. Locked slots are skipped and wraparound works.
5. Switching is blocked while reloading.
6. Missing HUD references or null weapon slots do not throw exceptions.
7. Existing weapon save/load still restores the selected weapon.

- [ ] **Step 4: Commit the complete validated feature**

```bash
git status --short
git log -1 --oneline
```

Confirm only intended feature files were committed and the working tree contains no accidental generated changes.
