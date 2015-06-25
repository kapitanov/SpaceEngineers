# Gravity Generator Changes

Gravity Generators have some new features in SECE.

## Technical Details

 * Added MyBoundedVector3 for neatly bundling fieldsize bounds.
 * Added .GetOrDefault(defaultValue) to floats and SerializableVector3s, making detection of uninitialized MyObjectBuilders a bit more standardized (uses NaN rather than negative or huge values).
 * MyGravityGeneratorBase:
   * Set to public, so modders can inherit it from their own classes.
   * Changed the idle noise to be configurable via `<PrimarySound>`, like reactors.
 * MyGravityGenerator (and friends):
   * Set to public, so modders can inherit it from their own classes.
   * `<FieldSize>` (MyBoundedVector3) added to definition and object-builder.
     * Default:
       * Min: <0f,0f,0f>
       * Max: <150f,150f,150f>
       * Default: <150f,150f,150f>
 * MyGravityGeneratorSphere (and friends):
   * Made public.
 * Added MyGravityProviderDefinition object builders to handle gravity acceleration-related serialization. Can eventually be worked into Gravity Spheres and whatnot instead of the current duplicated code.
   * `<Gravity>` (MyBounds) added to definition and object-builder.
     * Default: Min=-1f, Max=1f, Default=1f
   * Internally, Gravity Spheres do not share the same Gravity scaling as Gravity Generators ([-1,1] for Gravity Generators vs. [-G,G] for Gravity Spheres).  This presents a large problem for backwards-compatibility and will have to be tackled in a later commit.

## Step by Step

:note: These changes are only needed if you want to adjust the field size boundaries and defaults from the standard values listed above, otherwise they're automatically applied.

1. Crack open your CubeBlocks.sbc with your editor of choice.
2. Add the following to your Gravity Generators' `<Definition>`:
```xml
<FieldSize>
  <!-- Minimum field size -->
  <Min x="0" y="0" z="0" />
  <!-- Maximum field size -->
  <Max x="150" y="150" z="150" />
  <!-- Field size when gen is initially created -->
  <Default x="150" y="150" z="150" />
</FieldSize>
<!-- The same, but for the G's the generator puts out -->
<Gravity Min="-1" Max="1" Default="1" />
```
3. You can now adjust the field bounds and strengths to taste, even going beyond the prior 150m<sup>3</sup> maximum.
4. Save it, and you're done.  Objects already present in-world won't be changed (except for the min-max checks).
