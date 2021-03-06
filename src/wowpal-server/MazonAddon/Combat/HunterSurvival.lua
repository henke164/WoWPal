--[[
  Button    Spell
  mongooseBite = "1";
  coordinatedAssault = "2";
  killCommand = "3";
  wildfireBomb = "4";
  serpentSting = "5";
  viperSting = "6";
  breathOfTheDying = "7";
  mendPet = "9";
]]--

local mongooseBite = "1";
local coordinatedAssault = "2";
local killCommand = "3";
local wildfireBomb = "4";
local serpentSting = "5";
local viperSting = "6";
local breathOfTheDying = "7";
local wingClip = "8";
local mendPet = "9";

function RenderMultiTargetRotation()
  local wc = FindDebuff("target", "Wing Clip");
  if wc == nil and IsCastableAtEnemyTarget("Wing Clip", 0) then
    WowCyborg_CURRENTATTACK = "Wing Clip";
    return SetSpellRequest(wingClip);
  end
  return RenderSingleTargetRotation();
end

function RenderSingleTargetRotation()
  local petHp = GetHealthPercentage("pet");
  if tostring(petHp) ~= "-nan(ind)" and petHp > 1 and petHp < 90 then
    if IsCastable("Mend pet", 0) then
      WowCyborg_CURRENTATTACK = "Mend pet";
      return SetSpellRequest(mendPet);
    end
  end

  local mw = FindDebuff("target", "Mortal Wounds");
  local targetHp = GetHealthPercentage("target");
  if targetHp < 60 and mw == nil then
    if IsCastableAtEnemyTarget("Viper Sting", 0) then
      WowCyborg_CURRENTATTACK = "Viper Sting";
      return SetSpellRequest(viperSting);
    end
  end
  
  if targetHp < 80 then
    if IsCastableAtEnemyTarget("Reaping Flames", 0) then
      WowCyborg_CURRENTATTACK = "Reaping Flames";
      return SetSpellRequest(breathOfTheDying);
    end
  end

  local piBuff, piTime, piStacks = FindBuff("player", "Primeval Intuition");

  if (piStacks == nil or piStacks < 5) or piTime < 4 then
    if IsCastableAtEnemyTarget("Raptor Strike", 30) then
      WowCyborg_CURRENTATTACK = "Mongoose Bite";
      return SetSpellRequest(mongooseBite);
    end
  end

  if WowCyborg_INCOMBAT == false then
    return SetSpellRequest(nil);
  end
  
  local coordBuff = FindBuff("player", "Coordinated Assault");
  if coordBuff == nil and IsCastableAtEnemyTarget("Raptor Strike", 30) and IsCastable("Coordinated Assault", 0) then
    WowCyborg_CURRENTATTACK = "Coordinated Assault";
    return SetSpellRequest(coordinatedAssault);
  end

  local focus = UnitPower("player");
  if focus < 85 and IsCastableAtEnemyTarget("Kill Command", 0) then
    WowCyborg_CURRENTATTACK = "Kill Command";
    return SetSpellRequest(killCommand);
  end

  if IsCastableAtEnemyTarget("Wildfire Bomb", 0) then
    WowCyborg_CURRENTATTACK = "Wildfire Bomb";
    return SetSpellRequest(wildfireBomb);
  end
    
  if (piStacks == nil or piStacks < 5) and coordBuff == nil then
    if IsCastableAtEnemyTarget("Serpent Sting", 0) then
      WowCyborg_CURRENTATTACK = "Serpent Sting";
      return SetSpellRequest(serpentSting);
    end
  end

  if IsCastableAtEnemyTarget("Raptor Strike", 30) then
    WowCyborg_CURRENTATTACK = "Mongoose Bite";
    return SetSpellRequest(mongooseBite);
  end
  
  if focus > 60 and IsCastableAtEnemyTarget("Serpent Sting", 0) then
    WowCyborg_CURRENTATTACK = "Serpent Sting";
    return SetSpellRequest(serpentSting);
  end
end

print("Survival hunter rotation loaded");