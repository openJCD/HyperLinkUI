-- test lua file for 
-- well
-- testing?
function Init() 
	send_debug_message("Hello!")
	c = newContainer("TopRight", -10,-10, 50,50)
	c.DebugLabel = "LABEL!!!!!!";
	c:AddContainer(newContainer("Centre", 0, 0, 10, 10))
	Root:AddContainer(c)
	send_debug_message("Created new Container " .. c.DebugLabel .. " instance under UIRoot")
end

function OnUpdate()
end

function OnDraw()
end
