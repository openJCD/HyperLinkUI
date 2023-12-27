-- test lua file for 
-- well
-- testing?
function Init() 
	send_debug_message("Hello!")
	c = newContainer("BottomLeft", 10,-10, 400,50)
	c2 = newContainer("Centre", 0,0, 50,20)
	c.DebugLabel = "LABEL!!!!!!";
	c2.DebugLabel = "Label2!!"
	
	c:AddContainer(c2)
	Root:AddContainer(c)
	send_debug_message("Created new Container " .. c.DebugLabel .. " instance under UIRoot")
end

function OnUpdate()
end

function OnDraw()
end
