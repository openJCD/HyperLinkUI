-- test lua file for 
-- well
-- testing?
function Init() 
	send_debug_message("Hello!")
	c = newContainer("BottomLeft", 10,-10, 400,50)
	c2 = newContainer("TopRight", 0,0, 50,20)
	c.DebugLabel = "LABEL!!!!!!";
	c2.DebugLabel = "Label2"
	
	Root:AddContainer(c)
	Root:AddContainer(c2)
	send_debug_message("Created new Container " .. c2.DebugLabel .. " instance under " .. c2.Parent.DebugLabel)
end

function OnUpdate()
end

function OnDraw()
end
