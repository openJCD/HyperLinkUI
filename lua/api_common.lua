-- create an options dialog with settings (e.g. Fullscreen) and an apply button.
function create_standard_options_dialog(scrt, tag) 
    local d = new_window_container(scrt, "/root//Options", 50, -50, 200, 400, "BOTTOMRIGHT", tag)
    d.IsOpen = false; d.ClipContents=true;
    -- add buttons with "options_dialog_" prefix to distinguish. these can then be globally addressed in modules.
    options_dialog_cam_zoomin    = new_plain_button(d, "Zoom+0.1", 10, 30, 85, 40, "TOPLEFT", "None", "cam_zoom_+1")
    
    options_dialog_cam_zoomout   = new_plain_button(d, "Zoom-0.1", 10, 75, 85, 40, "TOPLEFT", "None", "cam_zoom_-1")
    
    -- parent, text, x, y, btnw, btnh, anchor, tag
    options_dialog_cam_reset     = new_plain_button(d, "Reset Zoom", 10, 120, 120, 40, "TOPLEFT", "None", "cam_zoom_reset")

    options_dialog_input_width   = new_text_input(d, "Width", 10, 165, 80, "TOPLEFT") 
    options_dialog_input_width.Padding = 2
    options_dialog_input_width:EnableNineSlice(tx_textinput)

    options_dialog_input_height  = new_text_input(d, "Height", 100, 165, 80, "TOPLEFT") 
    options_dialog_input_height.Padding = 2
    options_dialog_input_height:EnableNineSlice(tx_textinput)
    
    options_dialog_tog_fscreen   = new_checkbox(d, "Fullscreen", 10, 200, 30, 30, "TOPLEFT", "toggle_fullscreen")
    
    options_dialog_apply         = new_plain_button(d, "Apply", -10, -10, 80, 40, "BOTTOMRIGHT", "None", "options_dialog_btn_apply")
    
    options_dialog_txt_zoomlevel = new_text_label(d, "Zoom: " .. world_camera.Zoom, -5, 30, "TOPRIGHT")

    -- standard OnButtonClick logic for this standard dialog. has to be explicitly called in OnButtonClick function.
    options_dialog_OnButtonClick = function (sender, e)
        if e.tag =="options_dialog_btn_apply" then
            game_graphics.IsFullScreen=options_dialog_tog_fscreen.State
            --game_graphics.PreferredBackBufferWidth = options_dialog_input_width.InputText
            --game_graphics.PreferredBackBufferHeight= options_dialog_input_height.InputText
            game_graphics:ApplyChanges()
        end
        if e.tag == "dialog_options" and not d.IsOpen then
            staggered_custom({options_dialog_cam_zoomin, 
                options_dialog_cam_zoomout, 
                options_dialog_cam_reset, 
                options_dialog_tog_fscreen}, -200, 0, 0.5, 0.15, ease.inCubic);
            staggered_custom({
                options_dialog_txt_zoomlevel, 
                options_dialog_apply
            }, 200, 0, 0.5, 0.35, ease.inCubic);
        end
    end
    d:EnableCloseButton(10)
    return d;
end


function round(num, numDecimalPlaces)
  local mult = 10^(numDecimalPlaces or 0)
  return math.floor(num * mult + 0.5) / mult
end

-- tweening

ease = {
    linear = get_ease_func("Linear"),
    outCubic = get_ease_func("OutCubic"),
    inCubic = get_ease_func("InCubic"),
    inOutCubic = get_ease_func("InOutCubic"),
}