frameSize = 5;

function CreateDefaultFrame(x, y, width, height)
  local frame = CreateFrame("Frame");
  frame:ClearAllPoints();
  frame:SetPoint("BOTTOMLEFT", UIParent, "BOTTOMLEFT", x, y);
  frame:SetWidth(width);
  frame:SetHeight(height);
  local texture = frame:CreateTexture("WhiteTexture", "ARTWORK");
  texture:SetWidth(width);
  texture:SetHeight(height);
  texture:ClearAllPoints();
  texture:SetColorTexture(0, 0, 0);
  texture:SetAllPoints(frame);
  return frame, texture;
end

function CreateWrapperFrame()
  local frame, texture = CreateDefaultFrame(0, 0, frameSize * 4, frameSize);
  texture:SetColorTexture(1, 0, 0.5);
end

function CreateCombatFrame()
  local frame, texture = CreateDefaultFrame(0, frameSize, frameSize, frameSize);
  frame:RegisterEvent("PLAYER_REGEN_DISABLED");
  frame:RegisterEvent("PLAYER_REGEN_ENABLED");

  frame:SetScript("OnEvent", function(self, event, ...)
    if event == "PLAYER_REGEN_DISABLED" then
      texture:SetColorTexture(0, 1, 0);
    end
    if event == "PLAYER_REGEN_ENABLED" then
      texture:SetColorTexture(1, 0, 0);
    end
  end)
end

function TargetIsAlive()
  hp = UnitHealth("target");
  return hp > 0;
end

function CreateRangeCheckFrame()
  local frame, texture = CreateDefaultFrame(frameSize, frameSize, frameSize, frameSize);
  frame:SetScript("OnUpdate", function(self, event, ...)
    texture:SetColorTexture(1, 0, 0);
    if CheckInteractDistance("target", 4) then
      if (UnitCanAttack("player","target")) then
	    if TargetIsAlive() then
		  texture:SetColorTexture(0, 1, 0);
		end;
      end
    end
  end)
end

CreateCombatFrame();
CreateRangeCheckFrame();
CreateRotationFrame();
CreateWrapperFrame();