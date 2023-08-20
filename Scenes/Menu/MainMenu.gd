extends Control
@onready var StartMenu = $StartMenu
@onready var SettingsMenu = $SettingsMenu

# Starting menu
func _on_play_pressed():
	get_tree().change_scene_to_file("res://Scenes/TestLevel.tscn")

func _on_settings_pressed():
	StartMenu.hide()
	SettingsMenu.show()


func _on_exit_pressed():
	get_tree().quit()


#Setting Menu

# Video Settings

func _on_window_dropdown_item_selected(index):
	if index == 0: # Default
		DisplayServer.window_set_mode(0) # Windowed
	elif index == 1:
		DisplayServer.window_set_mode(3) # Fullscreen


func _on_resolution_dropdown_item_selected(index):
	
	if index == 0:
		DisplayServer.window_set_size(Vector2i(1280,720))
	if index == 1:
		DisplayServer.window_set_size(Vector2i(1920,1080))
	if index == 2:
		DisplayServer.window_set_size(Vector2i(2560,1440))
	if index == 3:
		DisplayServer.window_set_size(Vector2i(3840,2160))
	

func _on_VSync_dropdown_item_selected(index):
	# Vsync is enabled by default.
	# Vertical synchronization locks framerate and makes screen tearing not visible at the cost of
	# higher input latency and stuttering when the framerate target is not met.
	# Adaptive V-Sync automatically disables V-Sync when the framerate target is not met, and enables
	# V-Sync otherwise. This prevents suttering and reduces input latency when the framerate target
	# is not met, at the cost of visible tearing.
	if index == 0: # Disabled (default)
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_DISABLED)
	elif index == 1: # Adaptive
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_ADAPTIVE)
	elif index == 2: # Enabled
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_ENABLED)


# Audio Settings

func _on_master_value_changed(value):
	AudioServer.set_bus_volume_db(0,linear_to_db(value)) # Master Bus Index 0


func _on_music_value_changed(value):
	AudioServer.set_bus_volume_db(1,linear_to_db(value)) # Music Bus Index 1


func _on_sound_fx_value_changed(value):
	AudioServer.set_bus_volume_db(2,linear_to_db(value)) # FX Bus Index 2


#Back Button

func _on_back_pressed():
	SettingsMenu.hide()
	StartMenu.show()

