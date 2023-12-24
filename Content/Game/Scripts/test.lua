import ("VESSEL_GUI", "VESSEL_GUI.GUI.Containers", "VESSEL_GUI.GUI.Interfaces")
function Init() 
	testcontainer = Container(Root, 10, 10, 50, 100)
	testcontainer.IsSticky = true;
	testcontainer.DebugLabel = "LuaTestContainer"
end
