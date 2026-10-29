extends Control

@onready var draggableNodePaths : Dictionary = {}


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

# hash the node
# add to dictionary
# for this to work I need to get the socket children's hash via mousing over
func _add_draggable_node():
	pass
	
func _remove_draggable_node():
	pass

func on_element_moused_over():
	pass

# Draggable Socket Signals
#===========================================================================#

# Deck Socket
func _on_deck_socket_drag(control: Control) -> void:
	# check game state
	# consumes next card from deck stack
	# based on game state consume other resources
	# i.e, if already drawn this turn consume rapport
	pass # Replace with function body.


func _on_deck_socket_drop() -> void:
	# if card is played anywhere other than discard pile or play zone
	# place card into hand
	pass # Replace with function body.
