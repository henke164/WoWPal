--[[
  Button    Spell
  Ctrl+1    Macro for following focus "/follow focus"
  Ctrl+2    Macro for assisting focus "/assist focus"
  Ctrl+3    Mount
]]--

local startedFollowingAt = 0;
local startedAssistAt = 0;
local startedWaitAt = 0;
local serpentSting = "1";
local attack = "2";
local raptorStrike = "3";
local arcaneShot = "4";
local feedPet = "8";
local eat = "9";
local follow = "CTRL+1";
local assist = "CTRL+2";
local mount = "CTRL+3";
local back = "CTRL+9";
local aspectOfTheHawk = "SHIFT+1";
local huntersMark = "SHIFT+2";

function IsMelee()
  return CheckInteractDistance("target", 5);
end

function RenderMultiTargetRotation()
  return SetSpellRequest(nil);
end

function RenderSingleTargetRotation()
  if startedFollowingAt > GetTime() - 0.5 then
    WowCyborg_CURRENTATTACK = "Following...";
    return SetSpellRequest(follow);
  end

  if startedAssistAt > GetTime() - 0.5 then
    WowCyborg_CURRENTATTACK = "Assisting...";
    return SetSpellRequest(assist);
  end
  
  if startedWaitAt > GetTime() - 0.5 then
    WowCyborg_CURRENTATTACK = "Waiting...";
    return SetSpellRequest(back);
  end
    
  local hp = GetHealthPercentage("player");

  local aothBuff = FindBuff("player", "Aspect of the Hawk");
  if aothBuff == nil then
    if IsCastableAtEnemyTarget("Aspect of the Hawk", 20) then
      WowCyborg_CURRENTATTACK = "Aspect of the Hawk";
      return SetSpellRequest(aspectOfTheHawk);
    end
  end

  if IsMelee() ~= true then
    local hmDebuff = FindDebuff("target", "Hunter's Mark");
    if hmDebuff == nil then
      if IsCastableAtEnemyTarget("Hunter's Mark", 15) then
        WowCyborg_CURRENTATTACK = "Hunter's Mark";
        return SetSpellRequest(huntersMark);
      end
    end

    local ssDebuff = FindDebuff("target", "Serpent Sting");
    if ssDebuff == nil then
      if IsCastableAtEnemyTarget("Serpent Sting", 15) then
        WowCyborg_CURRENTATTACK = "Serpent Sting";
        return SetSpellRequest(serpentSting);
      end
    end

    if IsCastableAtEnemyTarget("Arcane Shot", 15) then
      WowCyborg_CURRENTATTACK = "Arcane Shot";
      return SetSpellRequest(arcaneShot);
    end

    if IsCastableAtEnemyTarget("Auto Shot", 0) and IsCurrentSpell(75) == false then
      WowCyborg_CURRENTATTACK = "Attack";
      return SetSpellRequest(attack);
    end

    WowCyborg_CURRENTATTACK = "-";
    return SetSpellRequest(nil);
  end
  
  if IsMelee() == true then
    if IsCastableAtEnemyTarget("Raptor Strike", 0) then
      WowCyborg_CURRENTATTACK = "Raptor Strike";
      return SetSpellRequest(raptorStrike);
    end
  end

  WowCyborg_CURRENTATTACK = "-";
  return SetSpellRequest(nil);
end

function CreateEmoteListenerFrame()
  local frame = CreateFrame("Frame");
  frame:RegisterEvent("CHAT_MSG_TEXT_EMOTE");
  frame:SetScript("OnEvent", function(self, event, ...)
    command = ...;
    if string.find(command, "follow", 1, true) then
      print("Following");
      startedFollowingAt = GetTime();
    end
    if string.find(command, "wait", 1, true) then
      print("Waiting");
      startedAssistAt = GetTime();
    end
    if string.find(command, "fart", 1, true) then
      print("Mounting");
      SetSpellRequest(mount);
    end
    if string.find(command, "waves", 1, true) then
      print("Fall back");
      startedWaitAt = GetTime();
    end
  end)
end

print("Classic hunter rotation loaded");
CreateEmoteListenerFrame();