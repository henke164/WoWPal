--[[
  Button    Spell
  bladeDance = "1";
  chaosStrike = "2";
  attack = "3";
  eyeBeam = "4";
  chaosNova = "5";
  concentratedFlame = "6";
  glavies = "8";
]]--

local bladeDance = "1";
local chaosStrike = "2";
local attack = "3";
local eyeBeam = "4";
local chaosNova = "5";
local concentratedFlame = "6";
local glavies = "8";
local felblade = "9";

function RenderMultiTargetRotation()
  return RenderSingleTargetRotation()
end

function RenderSingleTargetRotation()
  if InMeleeRange() == false then
    WowCyborg_CURRENTATTACK = "-";
    return SetSpellRequest(nil);
  end

  local thorns = FindBuff("target", "Thorns");
  if thorns ~= nil then
    WowCyborg_CURRENTATTACK = "Thorns";
    return SetSpellRequest(nil);
  end
  
  if IsCurrentSpell(6603) == false then
    WowCyborg_CURRENTATTACK = "Attack";
    return SetSpellRequest(attack);
  end

  if UnitChannelInfo("player") == "Eye Beam" then
    WowCyborg_CURRENTATTACK = "-";
    return SetSpellRequest(nil);
  end

  if IsCastableAtEnemyTarget("Concentrated Flame", 0) then
    WowCyborg_CURRENTATTACK = "Concentrated Flame";
    return SetSpellRequest(concentratedFlame);
  end

  if IsCastableAtEnemyTarget("Eye Beam", 30) then
    WowCyborg_CURRENTATTACK = "Eye Beam";
    return SetSpellRequest(eyeBeam);
  end

  if IsCastableAtEnemyTarget("Death Sweep", 15) or IsCastableAtEnemyTarget("Blade Dance", 15) then
    WowCyborg_CURRENTATTACK = "Blade Dance";
    return SetSpellRequest(bladeDance);
  end

  if IsCastableAtEnemyTarget("Chaos Strike", 70) or IsCastableAtEnemyTarget("Annihilation", 70) then
    WowCyborg_CURRENTATTACK = "Chaos Strike";
    return SetSpellRequest(chaosStrike);
  end

  if IsCastableAtEnemyTarget("Throw Glaive", 0) then
    WowCyborg_CURRENTATTACK = "Throw Glaive";
    return SetSpellRequest(glavies);
  end
  
  if IsCastableAtEnemyTarget("Felblade", 0) then
    WowCyborg_CURRENTATTACK = "Felblade";
    return SetSpellRequest(felblade);
  end
  
  WowCyborg_CURRENTATTACK = "-";
  return SetSpellRequest(nil);
end

function InMeleeRange()
  return IsSpellInRange("Chaos Strike", "target") == 1;
end

print("Demon hunter havoc rotation loaded");