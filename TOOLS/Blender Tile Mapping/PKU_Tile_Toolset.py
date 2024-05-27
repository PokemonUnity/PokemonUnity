bl_info = {
	"name": "Pokemon Tile Toolset",
	"description": "Toolset for easy create map assets for Unity 3D with Pokemon framework",
	"author": "Herbert Milhomme",
	"version": (1, 0, 0),
	"blender": (2, 7, 9),
	"location": "3D View > Object Mode > Toolshelf > Pokemon Tile Toolset",
	"category": "Object"
}

import bpy
import os
import bmesh
import math
import json
from mathutils import Vector
from bpy.props import StringProperty
from bpy.types import Operator


#-------------------------------------------------------
# Pokemon Tile-Export
# Pokemon Export to JSON...
class Multi_Poke_Tile_export(bpy.types.Operator):
	"""Export Pokemon Tiles to Unity"""
	bl_idname = "object.multi_poke_tile_export"
	bl_label = "Export Pokemon Tiles to Unity"
	bl_options = {'REGISTER', 'UNDO'}

	def convert_ob(self, ob):
		name, vector, euler, material = ob
		#vector = [float(coord) for coord in vector[1:-1].split(',')]
		#euler = [float(coord) for coord in euler.split(',')]
		vector = vector.replace('<Vector (', '').replace(')>', '')						# Remove '<Vector (' and ')>'
		euler = euler.replace('<Euler (', '').replace('), order=\'XYZ\'>', '')			# Remove '<Euler (' and '), order=\'XYZ\'>'
		euler = euler.replace('x=', '').replace('y=', '').replace('z=', '')				# Remove '[x,y,z]=' before float value
		material = material.replace('<bpy_struct, Material(\"', '').replace('\")>', '') # Remove '<bpy_struct, Material(\"' and '\")>'
		vector = [float(coord) for coord in vector.split(',')]
		euler = [float(coord) for coord in euler.split(',')]
		#return [name, vector, euler, material]
		return {"mesh": name, "vector": vector, "rotation": euler, "material": material}

	def execute(self, context):
		blend_not_saved = False
		#check saved blend file
		if len(bpy.data.filepath) == 0:
			self.report({'INFO'}, 'Objects don\'t export, because Blend file is not saved')
			blend_not_saved = True
		if blend_not_saved == False:
			# Save selected objects
			current_selected_obj = bpy.context.selected_objects
			current_unit_system = bpy.context.scene.unit_settings.system
			name = bpy.context.active_object.name
			bpy.context.scene.unit_settings.system = 'METRIC' #'NONE'
			bpy.context.scene.unit_settings.scale_length = 1
			#Export as JSON 
			if True:#context.scene.export_unity_json == True:
				current_pivot_point = bpy.context.space_data.pivot_point
				bpy.ops.view3d.snap_cursor_to_center()
				#for x in current_selected_obj:
					# Select only current object
				#	bpy.ops.object.select_all(action='DESELECT')
				#	if x.type == 'MESH':
				#		x.select = True
				#		# X-rotation fix
				#		bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)
				#		bpy.ops.transform.rotate(value = -1.5708, axis = (1, 0, 0), constraint_axis = (True, False, False), constraint_orientation = 'GLOBAL')
				#		bpy.ops.object.transform_apply(location=False, rotation=True, scale=False)
				#		bpy.ops.transform.rotate(value = 1.5708, axis = (1, 0, 0), constraint_axis = (True, False, False), constraint_orientation = 'GLOBAL')
				# Scale Fix
				#bpy.ops.object.select_all(action='DESELECT')
				#bpy.context.space_data.pivot_point = 'CURSOR'
				#for x in current_selected_obj:
				#	if x.type == 'MESH':
				#		x.select = True
				#bpy.ops.transform.resize(value=(100, 100, 100)) #No resize needed at 1=1 BlenderUnits
				#bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
				
				#Create export folder
				path = bpy.path.abspath('//JSON/')
				if not os.path.exists(path):
					os.makedirs(path)
				#deselect all and select only meshes
				bpy.ops.object.select_all(action='DESELECT')
				obs = []
				for x in current_selected_obj:
					if x.type == 'MESH':
						x.select = True
						obs.append([x.data.name, str(x.matrix_world.to_translation()), str(x.matrix_world.to_euler()), str(x.data.materials[0])]) #join objects to an array
						#obs.append([x.data.name, x.matrix_world.to_translation().X, x.matrix_world.to_translation().Y, x.matrix_world.to_translation().Z]) #join objects to an array
						#materials = [m for m in current_selected_obj.data.materials if m.groups] #one material per obj
						#vertices = [v for v in current_selected_obj.data.vertices if v.groups] #one vertex group per obj
						#vs = [ v for v in o.data.vertices if vg_idx in [ vg.group for vg in v.groups ] ]
				obs_converted = [self.convert_ob(ob) for ob in obs]

				# Calculate the width and height based on the x and y coordinates of the vectors
				#width = max(tile[1][0] for tile in obs_converted) - min(tile[1][0] for tile in obs_converted) + 1
				#height = max(tile[1][1] for tile in obs_converted) - min(tile[1][1] for tile in obs_converted) + 1
				width = max(tile["vector"][0] for tile in obs_converted) - min(tile["vector"][0] for tile in obs_converted) + 1
				height = max(tile["vector"][1] for tile in obs_converted) - min(tile["vector"][1] for tile in obs_converted) + 1

				# Convert width and height to integers
				width = int(width)
				height = int(height)

				output_data = {
					"width": width,
					"height": height,
					"tiles": obs_converted
				}
				with open(str(path + name + '.json'), 'w') as wfile:
					wfile.write(json.dumps(output_data, sort_keys=False, indent=4))
					#wfile.write(json.dumps(obs, sort_keys=False, indent=4))
					#wfile.write(str(obs))
				#Export as one Tile
				#bpy.ops.export_scene.fbx(filepath=str(path + name + '.fbx'), version='BIN7400', ui_tab='MAIN', use_selection=True, global_scale=1, apply_unit_scale=True)
				#Apply rotation and scale
				#bpy.ops.transform.resize(value=(0.01, 0.01, 0.01))
				#bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)
				bpy.context.space_data.pivot_point = current_pivot_point
			# Select again objects
			for j in current_selected_obj:
				j.select = True;
			bpy.context.scene.unit_settings.scale_length = 1
			bpy.context.scene.unit_settings.system = current_unit_system
		
		return {'FINISHED'}
# Feature let's you edit multiple objects to share same properties (mesh, materials, textures)
class MYADDON_OT_replace_mesh_material(bpy.types.Operator):
	bl_idname = "object.replace_properties_operator"
	bl_label = "Replace Properties"
	bl_options = {'REGISTER', 'UNDO'}
	clear_materials = bpy.props.BoolProperty(
		name="Clear Materials",
		description="If true, clear all materials from the object before assigning the new one",
		default=False
	)

	@classmethod
	def poll(cls, context):
		return context.active_object is not None

	def execute(self, context):
		# Get the mesh, texture, and material from the current scene
		#source_mesh = bpy.data.meshes[context.scene.replace_mesh_name]
		#source_texture = bpy.data.textures[context.scene.replace_texture_name]
		#source_material = bpy.data.materials[context.scene.replace_material_name]

		if context.scene.replace_mesh_name:  # Checks if a mesh name is selected
			source_mesh = bpy.data.meshes[context.scene.replace_mesh_name]
			if context.scene.replace_mesh_and_reposition_with_offset == True:
				#Create export folder
				path = bpy.path.abspath('//log/')
				if not os.path.exists(path):
					os.makedirs(path)
				# Open the log file
				with open('blender_log.txt', 'w') as log_file:
					# Get all selected objects
					for obj in bpy.context.selected_objects:
						# Check if the object is a mesh
						if obj.type == 'MESH':
							# Calculate the object's current origin in world space
							current_origin_world = obj.matrix_world.to_translation()        
							# Write the old origin and location values to the log file
							log_file.write('Old origin: {}\n'.format(current_origin_world))
							log_file.write('Old location: {}\n'.format(obj.location))
							# Calculate the mesh's bounding box center in object space
							mesh_bbox_center_object = sum((Vector(b) for b in obj.bound_box), Vector()) / 8
							# Convert the bounding box center to world space
							mesh_bbox_center_world = obj.matrix_world * mesh_bbox_center_object
							# Calculate the offset from the object's origin to the mesh's bounding box center
							offset = mesh_bbox_center_world - current_origin_world
							# Round the offset components to the nearest integers
							offset = Vector((math.floor(coordinate) for coordinate in offset))
							# Move the object by the offset
							obj.location += offset
							# Replace the object's mesh data with the shared mesh data
							obj.data = source_mesh        
							# Write the new origin and location values to the log file
							log_file.write('Offset: {}\n'.format(offset))
							log_file.write('New location: {}\n\n'.format(obj.location))
			else: 
				for obj in bpy.context.selected_objects:
					if obj.type == 'MESH':
						obj.data = source_mesh

		if context.scene.replace_material_name:  # Checks if a material name is selected
			source_material = bpy.data.materials[context.scene.replace_material_name]
			for obj in bpy.context.selected_objects:
				if obj.type == 'MESH':
					if self.clear_materials:
						obj.data.materials.clear()
					obj.data.materials.append(source_material)

		# Iterate over the selected objects
		#for obj in bpy.context.selected_objects:
		#	# Check if the object is a mesh object
		#	if obj.type == 'MESH':
		#		# Replace the object's mesh data with the source mesh data
		#		obj.data = source_mesh
		#
		#		# Replace the object's material
		#		if obj.data.materials:
		#			obj.data.materials[0] = source_material
		#		else:
		#			obj.data.materials.append(source_material)
		#
		#		# Replace the object's texture
		#		for slot in obj.material_slots:
		#			for node in slot.material.node_tree.nodes:
		#				if node.type == 'TEX_IMAGE':
		#					node.image = source_texture

		return {'FINISHED'}
# UI for above feature to appear in panel on 3D view
#class MYADDON_PT_main_panel(bpy.types.Panel):
#	"""Creates a Panel in the Object properties window"""
#	bl_label = "Override Selected Objects"
#	bl_idname = "OBJECT_PT_replace_properties"
#	bl_space_type = 'VIEW_3D'
#	bl_region_type = "TOOLS"
#	bl_category = 'Tool'
#
#	def draw(self, context):
#		layout = self.layout
#		if context.object is not None:
#			if context.mode == 'OBJECT':
#				layout.prop_search(context.scene, 'replace_mesh_name', bpy.data, 'meshes')
#				#layout.prop_search(context.scene, 'replace_texture_name', bpy.data, 'textures')
#				layout.prop_search(context.scene, 'replace_material_name', bpy.data, 'materials')
#				#layout.prop(context.window_manager.replace_properties_operator, "clear_materials")
#				#layout.operator('object.replace_properties_operator')
#				layout.prop(MYADDON_OT_replace_mesh_material, "clear_materials")
#				layout.operator(MYADDON_OT_replace_mesh_material.bl_idname, text="Replace/Assign")

# Unregister the class first, if it's already registered
#if MYADDON_OT_replace_mesh_material in bpy.utils.registered_classes():
#    bpy.utils.unregister_class(MYADDON_OT_replace_mesh_material)
#if MYADDON_PT_main_panel in bpy.utils.registered_classes():
#    bpy.utils.unregister_class(MYADDON_PT_main_panel)
		
#-------------------------------------------------------
# Panels
class VIEW3D_PT_ImportExport_PKUTools_panel(bpy.types.Panel):
	bl_label = "Export Pokemon-Unity Tools"
	bl_space_type = "VIEW_3D"
	bl_region_type = "TOOLS"
	bl_options = {'DEFAULT_CLOSED'}

	@classmethod
	def poll(self, context):
		return (True)

	def draw(self, context):
		layout = self.layout	
		if context.object is not None:
			if context.mode == 'OBJECT':
				row = layout.row()
				row.operator("object.multi_poke_tile_export", text="Export Tiles to Unity")
				#layout.prop(context.scene, "export_unity_json", text="Export Objects to JSON")
				layout.separator()
		
			if context.mode == 'OBJECT':
				layout.prop_search(context.scene, 'replace_mesh_name', bpy.data, 'meshes')
				layout.prop(context.scene, "replace_mesh_and_reposition_with_offset", text="Calculate Offset")
				#layout.prop_search(context.scene, 'replace_texture_name', bpy.data, 'textures')
				layout.prop_search(context.scene, 'replace_material_name', bpy.data, 'materials')
				#layout.prop(context.window_manager.replace_properties_operator, "clear_materials")
				#layout.prop(MYADDON_OT_replace_mesh_material, "clear_materials")
				layout.operator('object.replace_properties_operator', text="Replace/Assign")
				#layout.operator(MYADDON_OT_replace_mesh_material.bl_idname, text="Replace/Assign")
		
		#if context.mode == 'OBJECT':
		#	row = layout.row()
		#	row.operator("object.export_unity_json", text="Export Objects to JSON")
				

#-------------------------------------------------------		
def register():
	bpy.utils.register_module(__name__)
	bpy.types.Scene.replace_mesh_and_reposition_with_offset = bpy.props.BoolProperty(
		name="Calculate Offset",
		description="When changing Mesh, will reposition object location to match the changes",
		default = True)
	
	#bpy.types.Scene.export_unity_json = bpy.props.BoolProperty(
	#	name="Export Objects to JSON",
	#	description="Export Selected Objects to a JSON file that will be used for importing in custom unity script",
	#	default = False)
	
	# Below is for bulk edit objects en masse
	bpy.types.Scene.replace_mesh_name = bpy.props.StringProperty(
		name="Mesh",
		description="Select a mesh to use",
		default=""
	)
	bpy.types.Scene.replace_texture_name = bpy.props.StringProperty(
		name="Texture",
		description="Select a texture to use",
		default=""
	)
	bpy.types.Scene.replace_material_name = bpy.props.StringProperty(
		name="Material",
		description="Select a material to use",
		default=""
	)
	#bpy.utils.register_class(MYADDON_OT_replace_mesh_material)
	#bpy.utils.register_class(MYADDON_PT_main_panel)
	
def unregister():
	bpy.utils.unregister_module(__name__)
	del bpy.types.Scene.replace_mesh_and_reposition_with_offset
	#del bpy.types.Scene.export_unity_json
	#below is for bulk editting...
	del bpy.types.Scene.replace_mesh_name
	del bpy.types.Scene.replace_texture_name
	del bpy.types.Scene.replace_material_name
	#bpy.utils.unregister_class(MYADDON_OT_replace_mesh_material)
	#bpy.utils.unregister_class(MYADDON_PT_main_panel)

if __name__ == "__main__":
	register()