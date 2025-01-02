BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "pokemon_form_pokeathlon_stats" (
	"pokemon_form_id"	INTEGER NOT NULL,
	"pokeathlon_stat_id"	INTEGER NOT NULL,
	"minimum_stat"	INTEGER NOT NULL,
	"base_stat"	INTEGER NOT NULL,
	"maximum_stat"	INTEGER NOT NULL,
	PRIMARY KEY("pokemon_form_id","pokeathlon_stat_id"),
	FOREIGN KEY("pokemon_form_id") REFERENCES "pokemon_forms"("id"),
	FOREIGN KEY("pokeathlon_stat_id") REFERENCES "pokeathlon_stats"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_form_names" (
	"pokemon_form_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"form_name"	VARCHAR(79),
	"pokemon_name"	VARCHAR(79),
	PRIMARY KEY("pokemon_form_id","local_language_id"),
	FOREIGN KEY("pokemon_form_id") REFERENCES "pokemon_forms"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_form_generations" (
	"pokemon_form_id"	INTEGER NOT NULL,
	"generation_id"	INTEGER NOT NULL,
	"game_index"	INTEGER NOT NULL,
	PRIMARY KEY("pokemon_form_id","generation_id"),
	FOREIGN KEY("generation_id") REFERENCES "generations"("id"),
	FOREIGN KEY("pokemon_form_id") REFERENCES "pokemon_forms"("id")
);
CREATE TABLE IF NOT EXISTS "encounter_condition_value_map" (
	"encounter_id"	INTEGER NOT NULL,
	"encounter_condition_value_id"	INTEGER NOT NULL,
	FOREIGN KEY("encounter_condition_value_id") REFERENCES "encounter_condition_values"("id"),
	PRIMARY KEY("encounter_id","encounter_condition_value_id"),
	FOREIGN KEY("encounter_id") REFERENCES "encounters"("id")
);
CREATE TABLE IF NOT EXISTS "encounters" (
	"id"	INTEGER NOT NULL,
	"version_id"	INTEGER NOT NULL,
	"location_area_id"	INTEGER NOT NULL,
	"encounter_slot_id"	INTEGER NOT NULL,
	"pokemon_id"	INTEGER NOT NULL,
	"min_level"	INTEGER NOT NULL,
	"max_level"	INTEGER NOT NULL,
	FOREIGN KEY("location_area_id") REFERENCES "location_areas"("id"),
	UNIQUE("id"),--PRIMARY KEY("id"),
	PRIMARY KEY("location_area_id", "encounter_slot_id"),
	FOREIGN KEY("version_id") REFERENCES "versions"("id"),
	FOREIGN KEY("encounter_slot_id") REFERENCES "encounter_slots"("id"),
	FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_types" (
	"pokemon_id"	INTEGER NOT NULL,
	"type_id"	INTEGER NOT NULL,
	"slot"	INTEGER NOT NULL,
	PRIMARY KEY("pokemon_id","slot"),
	UNIQUE("pokemon_id","slot"),
	FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id"),
	FOREIGN KEY("type_id") REFERENCES "types"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_moves" (
	"pokemon_id"	INTEGER NOT NULL,
	"version_group_id"	INTEGER NOT NULL,
	"move_id"	INTEGER NOT NULL,
	"pokemon_move_method_id"	INTEGER NOT NULL,
	"level"	INTEGER NOT NULL,
	"order"	INTEGER,
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id"),
	PRIMARY KEY("pokemon_id","version_group_id","move_id","pokemon_move_method_id","level"),
	FOREIGN KEY("pokemon_move_method_id") REFERENCES "pokemon_move_methods"("id"),
	FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_game_indices" (
	"pokemon_id"	INTEGER NOT NULL,
	"version_id"	INTEGER NOT NULL,
	"game_index"	INTEGER NOT NULL,
	FOREIGN KEY("version_id") REFERENCES "versions"("id"),
	PRIMARY KEY("pokemon_id","version_id"),
	FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_items" (
	"pokemon_id"	INTEGER NOT NULL,
	"version_id"	INTEGER NOT NULL,
	"item_id"	INTEGER NOT NULL,
	"rarity"	INTEGER NOT NULL,
	PRIMARY KEY("pokemon_id","version_id","item_id"),
	FOREIGN KEY("item_id") REFERENCES "items"("id"),
	FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id"),
	FOREIGN KEY("version_id") REFERENCES "versions"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_forms" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"form_identifier"	VARCHAR(79),
	"pokemon_id"	INTEGER NOT NULL,
	"introduced_in_version_group_id"	INTEGER,
	"is_default"	BOOLEAN NOT NULL,
	"is_battle_only"	BOOLEAN NOT NULL,
	"is_mega"	BOOLEAN NOT NULL,
	"form_order"	INTEGER NOT NULL,
	"order"	INTEGER NOT NULL,
	PRIMARY KEY("id"),
	CHECK(is_default IN (0,1)),
	CHECK(is_battle_only IN (0,1)),
	CHECK(is_mega IN (0,1)),
	FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id"),
	FOREIGN KEY("introduced_in_version_group_id") REFERENCES "version_groups"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_abilities" (
	"pokemon_id"	INTEGER NOT NULL,
	"ability_id"	INTEGER NOT NULL,
	"is_hidden"	BOOLEAN NOT NULL,
	"slot"	INTEGER NOT NULL,
	PRIMARY KEY("pokemon_id","slot"),
	UNIQUE("pokemon_id","slot"),
	CHECK(is_hidden IN (0,1)),
	FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id"),
	FOREIGN KEY("ability_id") REFERENCES "abilities"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_stats" (
	"pokemon_id"	INTEGER NOT NULL,
	"stat_id"	INTEGER NOT NULL,
	"base_stat"	INTEGER NOT NULL,
	"effort"	INTEGER NOT NULL,
	FOREIGN KEY("stat_id") REFERENCES "stats"("id"),
	UNIQUE("pokemon_id","stat_id"),
	PRIMARY KEY("pokemon_id","stat_id"),
	FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_egg_groups" (
	"species_id"	INTEGER NOT NULL,
	"egg_group_id"	INTEGER NOT NULL,
	PRIMARY KEY("species_id","egg_group_id"),
	FOREIGN KEY("species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("egg_group_id") REFERENCES "egg_groups"("id")
);
CREATE TABLE IF NOT EXISTS "pal_park" (
	"species_id"	INTEGER NOT NULL,
	"area_id"	INTEGER NOT NULL,
	"base_score"	INTEGER NOT NULL,
	"rate"	INTEGER NOT NULL,
	PRIMARY KEY("species_id"),
	FOREIGN KEY("species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("area_id") REFERENCES "pal_park_areas"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_species_flavor_text" (
	"species_id"	INTEGER NOT NULL,
	"version_id"	INTEGER NOT NULL,
	"language_id"	INTEGER NOT NULL,
	"flavor_text"	TEXT NOT NULL,
	PRIMARY KEY("species_id","version_id","language_id"),
	FOREIGN KEY("species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("version_id") REFERENCES "versions"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_species_names" (
	"pokemon_species_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	"genus"	TEXT,
	PRIMARY KEY("pokemon_species_id","local_language_id"),
	FOREIGN KEY("pokemon_species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_evolution" (
	"id"	INTEGER NOT NULL,
	"evolved_species_id"	INTEGER NOT NULL,
	"evolution_trigger_id"	INTEGER NOT NULL,
	"trigger_item_id"	INTEGER,
	"minimum_level"	INTEGER,
	"gender_id"	INTEGER,
	"location_id"	INTEGER,
	"held_item_id"	INTEGER,
	"time_of_day"	VARCHAR(5),
	"known_move_id"	INTEGER,
	"known_move_type_id"	INTEGER,
	"minimum_happiness"	INTEGER,
	"minimum_beauty"	INTEGER,
	"minimum_affection"	INTEGER,
	"relative_physical_stats"	INTEGER,
	"party_species_id"	INTEGER,
	"party_type_id"	INTEGER,
	"trade_species_id"	INTEGER,
	"needs_overworld_rain"	BOOLEAN NOT NULL,
	"turn_upside_down"	BOOLEAN NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("evolution_trigger_id") REFERENCES "evolution_triggers"("id"),
	FOREIGN KEY("evolved_species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("trigger_item_id") REFERENCES "items"("id"),
	FOREIGN KEY("gender_id") REFERENCES "genders"("id"),
	FOREIGN KEY("known_move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("held_item_id") REFERENCES "items"("id"),
	FOREIGN KEY("location_id") REFERENCES "locations"("id"),
	CONSTRAINT "pokemon_evolution_time_of_day" CHECK(time_of_day IN ('day','night')),
	CHECK(needs_overworld_rain IN (0,1)),
	CHECK(turn_upside_down IN (0,1)),
	FOREIGN KEY("known_move_type_id") REFERENCES "types"("id"),
	FOREIGN KEY("party_species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("party_type_id") REFERENCES "types"("id"),
	FOREIGN KEY("trade_species_id") REFERENCES "pokemon_species"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"species_id"	INTEGER,
	"height"	INTEGER NOT NULL,
	"weight"	INTEGER NOT NULL,
	"base_experience"	INTEGER NOT NULL,
	"order"	INTEGER NOT NULL,
	"is_default"	BOOLEAN NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("species_id") REFERENCES "pokemon_species"("id"),
	CHECK(is_default IN (0,1))
);
CREATE TABLE IF NOT EXISTS "pokemon_dex_numbers" (
	"species_id"	INTEGER NOT NULL,
	"pokedex_id"	INTEGER NOT NULL,
	"pokedex_number"	INTEGER NOT NULL,
	PRIMARY KEY("species_id","pokedex_id"),
	UNIQUE("pokedex_id","pokedex_number"),
	FOREIGN KEY("pokedex_id") REFERENCES "pokedexes"("id"),
	FOREIGN KEY("species_id") REFERENCES "pokemon_species"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_species_prose" (
	"pokemon_species_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"form_description"	TEXT,
	PRIMARY KEY("pokemon_species_id","local_language_id"),
	FOREIGN KEY("pokemon_species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_species_flavor_summaries" (
	"pokemon_species_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"flavor_summary"	TEXT,
	PRIMARY KEY("pokemon_species_id","local_language_id"),
	FOREIGN KEY("pokemon_species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "version_names" (
	"version_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("version_id","local_language_id"),
	FOREIGN KEY("version_id") REFERENCES "versions"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "move_changelog" (
	"move_id"	INTEGER NOT NULL,
	"changed_in_version_group_id"	INTEGER NOT NULL,
	"type_id"	INTEGER,
	"power"	SMALLINT,
	"pp"	SMALLINT,
	"accuracy"	SMALLINT,
	"priority"	SMALLINT,
	"target_id"	INTEGER,
	"effect_id"	INTEGER,
	"effect_chance"	INTEGER,
	PRIMARY KEY("move_id","changed_in_version_group_id"),
	FOREIGN KEY("effect_id") REFERENCES "move_effects"("id"),
	FOREIGN KEY("target_id") REFERENCES "move_targets"("id"),
	FOREIGN KEY("changed_in_version_group_id") REFERENCES "version_groups"("id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("type_id") REFERENCES "types"("id")
);
CREATE TABLE IF NOT EXISTS "move_flavor_text" (
	"move_id"	INTEGER NOT NULL,
	"version_group_id"	INTEGER NOT NULL,
	"language_id"	INTEGER NOT NULL,
	"flavor_text"	TEXT NOT NULL,
	PRIMARY KEY("move_id","version_group_id","language_id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id")
);
CREATE TABLE IF NOT EXISTS "ability_changelog_prose" (
	"ability_changelog_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"effect"	TEXT NOT NULL,
	PRIMARY KEY("ability_changelog_id","local_language_id"),
	FOREIGN KEY("ability_changelog_id") REFERENCES "ability_changelog"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "super_contest_combos" (
	"first_move_id"	INTEGER NOT NULL,
	"second_move_id"	INTEGER NOT NULL,
	PRIMARY KEY("first_move_id","second_move_id"),
	FOREIGN KEY("first_move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("second_move_id") REFERENCES "moves"("id")
);
CREATE TABLE IF NOT EXISTS "contest_combos" (
	"first_move_id"	INTEGER NOT NULL,
	"second_move_id"	INTEGER NOT NULL,
	PRIMARY KEY("first_move_id","second_move_id"),
	FOREIGN KEY("first_move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("second_move_id") REFERENCES "moves"("id")
);
CREATE TABLE IF NOT EXISTS "move_flag_map" (
	"move_id"	INTEGER NOT NULL,
	"move_flag_id"	INTEGER NOT NULL,
	PRIMARY KEY("move_id","move_flag_id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("move_flag_id") REFERENCES "move_flags"("id")
);
CREATE TABLE IF NOT EXISTS "move_meta_stat_changes" (
	"move_id"	INTEGER NOT NULL,
	"stat_id"	INTEGER NOT NULL,
	"change"	INTEGER NOT NULL,
	PRIMARY KEY("move_id","stat_id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("stat_id") REFERENCES "stats"("id")
);
CREATE TABLE IF NOT EXISTS "move_names" (
	"move_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("move_id","local_language_id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "location_area_encounter_rates" (
	"location_area_id"	INTEGER NOT NULL,
	"encounter_method_id"	INTEGER NOT NULL,
	"version_id"	INTEGER NOT NULL,
	"rate"	INTEGER,
	FOREIGN KEY("location_area_id") REFERENCES "location_areas"("id"),
	PRIMARY KEY("location_area_id","encounter_method_id","version_id"),
	FOREIGN KEY("encounter_method_id") REFERENCES "encounter_methods"("id"),
	FOREIGN KEY("version_id") REFERENCES "versions"("id")
);
CREATE TABLE IF NOT EXISTS "move_flavor_summaries" (
	"move_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"flavor_summary"	TEXT,
	PRIMARY KEY("move_id","local_language_id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "move_effect_changelog_prose" (
	"move_effect_changelog_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"effect"	TEXT NOT NULL,
	PRIMARY KEY("move_effect_changelog_id","local_language_id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("move_effect_changelog_id") REFERENCES "move_effect_changelog"("id")
);
CREATE TABLE IF NOT EXISTS "berry_flavors" (
	"berry_id"	INTEGER NOT NULL,
	"contest_type_id"	INTEGER NOT NULL,
	"flavor"	INTEGER NOT NULL,
	PRIMARY KEY("berry_id","contest_type_id"),
	FOREIGN KEY("berry_id") REFERENCES "berries"("id"),
	FOREIGN KEY("contest_type_id") REFERENCES "contest_types"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_species" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"generation_id"	INTEGER,
	"evolves_from_species_id"	INTEGER,
	"evolution_chain_id"	INTEGER,
	"color_id"	INTEGER NOT NULL,
	"shape_id"	INTEGER NOT NULL,
	"habitat_id"	INTEGER,
	"gender_rate"	INTEGER NOT NULL,
	"capture_rate"	INTEGER NOT NULL,
	"base_happiness"	INTEGER NOT NULL,
	"is_baby"	BOOLEAN NOT NULL,
	"hatch_counter"	INTEGER NOT NULL,
	"has_gender_differences"	BOOLEAN NOT NULL,
	"growth_rate_id"	INTEGER NOT NULL,
	"forms_switchable"	BOOLEAN NOT NULL,
	"order"	INTEGER NOT NULL,
	"conquest_order"	INTEGER,
	FOREIGN KEY("habitat_id") REFERENCES "pokemon_habitats"("id"),
	PRIMARY KEY("id"),
	FOREIGN KEY("evolves_from_species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("growth_rate_id") REFERENCES "growth_rates"("id"),
	FOREIGN KEY("generation_id") REFERENCES "generations"("id"),
	FOREIGN KEY("evolution_chain_id") REFERENCES "evolution_chains"("id"),
	CHECK(is_baby IN (0,1)),
	CHECK(has_gender_differences IN (0,1)),
	CHECK(forms_switchable IN (0,1)),
	FOREIGN KEY("color_id") REFERENCES "pokemon_colors"("id"),
	FOREIGN KEY("shape_id") REFERENCES "pokemon_shapes"("id")
);
CREATE TABLE IF NOT EXISTS "move_meta" (
	"move_id"	INTEGER NOT NULL,
	"meta_category_id"	INTEGER NOT NULL,
	"meta_ailment_id"	INTEGER NOT NULL,
	"min_hits"	INTEGER,
	"max_hits"	INTEGER,
	"min_turns"	INTEGER,
	"max_turns"	INTEGER,
	"drain"	INTEGER NOT NULL,
	"healing"	INTEGER NOT NULL,
	"crit_rate"	INTEGER NOT NULL,
	"ailment_chance"	INTEGER NOT NULL,
	"flinch_chance"	INTEGER NOT NULL,
	"stat_chance"	INTEGER NOT NULL,
	PRIMARY KEY("move_id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id"),
	FOREIGN KEY("meta_ailment_id") REFERENCES "move_meta_ailments"("id"),
	FOREIGN KEY("meta_category_id") REFERENCES "move_meta_categories"("id")
);
CREATE TABLE IF NOT EXISTS "machines" (
	"machine_number"	INTEGER NOT NULL,
	"version_group_id"	INTEGER NOT NULL,
	"item_id"	INTEGER NOT NULL,
	"move_id"	INTEGER NOT NULL,
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id"),
	PRIMARY KEY("machine_number","version_group_id"),
	FOREIGN KEY("item_id") REFERENCES "items"("id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id")
);
CREATE TABLE IF NOT EXISTS "characteristic_text" (
	"characteristic_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"message"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("characteristic_id","local_language_id"),
	FOREIGN KEY("characteristic_id") REFERENCES "characteristics"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "item_names" (
	"item_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id"),
	PRIMARY KEY("item_id","local_language_id"),
	FOREIGN KEY("item_id") REFERENCES "items"("id")
);
CREATE TABLE IF NOT EXISTS "item_flavor_text" (
	"item_id"	INTEGER NOT NULL,
	"version_group_id"	INTEGER NOT NULL,
	"language_id"	INTEGER NOT NULL,
	"flavor_text"	TEXT NOT NULL,
	PRIMARY KEY("item_id","version_group_id","language_id"),
	FOREIGN KEY("item_id") REFERENCES "items"("id"),
	FOREIGN KEY("language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id")
);
CREATE TABLE IF NOT EXISTS "type_efficacy" (
	"damage_type_id"	INTEGER NOT NULL,
	"target_type_id"	INTEGER NOT NULL,
	"damage_factor"	INTEGER NOT NULL,
	PRIMARY KEY("damage_type_id","target_type_id"),
	FOREIGN KEY("damage_type_id") REFERENCES "types"("id"),
	FOREIGN KEY("target_type_id") REFERENCES "types"("id")
);
CREATE TABLE IF NOT EXISTS "type_names" (
	"type_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("type_id","local_language_id"),
	FOREIGN KEY("type_id") REFERENCES "types"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "item_game_indices" (
	"item_id"	INTEGER NOT NULL,
	"generation_id"	INTEGER NOT NULL,
	"game_index"	INTEGER NOT NULL,
	FOREIGN KEY("generation_id") REFERENCES "generations"("id"),
	PRIMARY KEY("item_id","generation_id"),
	FOREIGN KEY("item_id") REFERENCES "items"("id")
);
CREATE TABLE IF NOT EXISTS "encounter_slots" (
	"id"	INTEGER NOT NULL,
	"version_group_id"	INTEGER NOT NULL,
	"encounter_method_id"	INTEGER NOT NULL,
	"slot"	INTEGER,
	"rarity"	INTEGER,
	PRIMARY KEY("id"),
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id"),
	FOREIGN KEY("encounter_method_id") REFERENCES "encounter_methods"("id")
);
CREATE TABLE IF NOT EXISTS "ability_names" (
	"ability_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("ability_id","local_language_id"),
	FOREIGN KEY("ability_id") REFERENCES "abilities"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "moves" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"generation_id"	INTEGER NOT NULL,
	"type_id"	INTEGER NOT NULL,
	"power"	SMALLINT,
	"pp"	SMALLINT,
	"accuracy"	SMALLINT,
	"priority"	SMALLINT NOT NULL,
	"target_id"	INTEGER NOT NULL,
	"damage_class_id"	INTEGER NOT NULL,
	"effect_id"	INTEGER NOT NULL,
	"effect_chance"	INTEGER,
	"contest_type_id"	INTEGER,
	"contest_effect_id"	INTEGER,
	"super_contest_effect_id"	INTEGER,
	PRIMARY KEY("id"),
	FOREIGN KEY("super_contest_effect_id") REFERENCES "super_contest_effects"("id"),
	FOREIGN KEY("damage_class_id") REFERENCES "move_damage_classes"("id"),
	FOREIGN KEY("generation_id") REFERENCES "generations"("id"),
	FOREIGN KEY("type_id") REFERENCES "types"("id"),
	FOREIGN KEY("target_id") REFERENCES "move_targets"("id"),
	FOREIGN KEY("effect_id") REFERENCES "move_effects"("id"),
	FOREIGN KEY("contest_type_id") REFERENCES "contest_types"("id"),
	FOREIGN KEY("contest_effect_id") REFERENCES "contest_effects"("id")
);
CREATE TABLE IF NOT EXISTS "versions" (
	"id"	INTEGER NOT NULL,
	"version_group_id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id")
);
CREATE TABLE IF NOT EXISTS "ability_flavor_text" (
	"ability_id"	INTEGER NOT NULL,
	"version_group_id"	INTEGER NOT NULL,
	"language_id"	INTEGER NOT NULL,
	"flavor_text"	TEXT NOT NULL,
	PRIMARY KEY("ability_id","version_group_id","language_id"),
	FOREIGN KEY("ability_id") REFERENCES "abilities"("id"),
	FOREIGN KEY("language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id")
);
CREATE TABLE IF NOT EXISTS "nature_pokeathlon_stats" (
	"nature_id"	INTEGER NOT NULL,
	"pokeathlon_stat_id"	INTEGER NOT NULL,
	"max_change"	INTEGER NOT NULL,
	PRIMARY KEY("nature_id","pokeathlon_stat_id"),
	FOREIGN KEY("nature_id") REFERENCES "natures"("id"),
	FOREIGN KEY("pokeathlon_stat_id") REFERENCES "pokeathlon_stats"("id")
);
CREATE TABLE IF NOT EXISTS "item_prose" (
	"item_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"short_effect"	TEXT,
	"effect"	TEXT,
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id"),
	PRIMARY KEY("item_id","local_language_id"),
	FOREIGN KEY("item_id") REFERENCES "items"("id")
);
CREATE TABLE IF NOT EXISTS "item_flag_map" (
	"item_id"	INTEGER NOT NULL,
	"item_flag_id"	INTEGER NOT NULL,
	FOREIGN KEY("item_flag_id") REFERENCES "item_flags"("id"),
	PRIMARY KEY("item_id","item_flag_id"),
	FOREIGN KEY("item_id") REFERENCES "items"("id")
);
CREATE TABLE IF NOT EXISTS "version_group_regions" (
	"version_group_id"	INTEGER NOT NULL,
	"region_id"	INTEGER NOT NULL,
	PRIMARY KEY("version_group_id","region_id"),
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id"),
	FOREIGN KEY("region_id") REFERENCES "regions"("id")
);
CREATE TABLE IF NOT EXISTS "pokedex_version_groups" (
	"pokedex_id"	INTEGER NOT NULL,
	"version_group_id"	INTEGER NOT NULL,
	FOREIGN KEY("pokedex_id") REFERENCES "pokedexes"("id"),
	PRIMARY KEY("pokedex_id","version_group_id"),
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id")
);
CREATE TABLE IF NOT EXISTS "nature_names" (
	"nature_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("nature_id","local_language_id"),
	FOREIGN KEY("nature_id") REFERENCES "natures"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "move_effect_changelog" (
	"id"	INTEGER NOT NULL,
	"effect_id"	INTEGER NOT NULL,
	"changed_in_version_group_id"	INTEGER NOT NULL,
	PRIMARY KEY("id"),
	UNIQUE("effect_id","changed_in_version_group_id"),
	FOREIGN KEY("changed_in_version_group_id") REFERENCES "version_groups"("id"),
	FOREIGN KEY("effect_id") REFERENCES "move_effects"("id")
);
CREATE TABLE IF NOT EXISTS "type_game_indices" (
	"type_id"	INTEGER NOT NULL,
	"generation_id"	INTEGER NOT NULL,
	"game_index"	INTEGER NOT NULL,
	PRIMARY KEY("type_id","generation_id"),
	FOREIGN KEY("type_id") REFERENCES "types"("id"),
	FOREIGN KEY("generation_id") REFERENCES "generations"("id")
);
CREATE TABLE IF NOT EXISTS "ability_prose" (
	"ability_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"short_effect"	TEXT,
	"effect"	TEXT,
	PRIMARY KEY("ability_id","local_language_id"),
	FOREIGN KEY("ability_id") REFERENCES "abilities"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "evolution_chains" (
	"id"	INTEGER NOT NULL,
	"baby_trigger_item_id"	INTEGER,
	PRIMARY KEY("id"),
	FOREIGN KEY("baby_trigger_item_id") REFERENCES "items"("id")
);
CREATE TABLE IF NOT EXISTS "item_flavor_summaries" (
	"item_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"flavor_summary"	TEXT,
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id"),
	PRIMARY KEY("item_id","local_language_id"),
	FOREIGN KEY("item_id") REFERENCES "items"("id")
);
CREATE TABLE IF NOT EXISTS "berries" (
	"id"	INTEGER NOT NULL,
	"item_id"	INTEGER NOT NULL,
	"firmness_id"	INTEGER NOT NULL,
	"natural_gift_power"	INTEGER,
	"natural_gift_type_id"	INTEGER,
	"size"	INTEGER NOT NULL,
	"max_harvest"	INTEGER NOT NULL,
	"growth_time"	INTEGER NOT NULL,
	"soil_dryness"	INTEGER NOT NULL,
	"smoothness"	INTEGER NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("natural_gift_type_id") REFERENCES "types"("id"),
	FOREIGN KEY("item_id") REFERENCES "items"("id"),
	FOREIGN KEY("firmness_id") REFERENCES "berry_firmness"("id")
);
CREATE TABLE IF NOT EXISTS "version_group_pokemon_move_methods" (
	"version_group_id"	INTEGER NOT NULL,
	"pokemon_move_method_id"	INTEGER NOT NULL,
	PRIMARY KEY("version_group_id","pokemon_move_method_id"),
	FOREIGN KEY("version_group_id") REFERENCES "version_groups"("id"),
	FOREIGN KEY("pokemon_move_method_id") REFERENCES "pokemon_move_methods"("id")
);
CREATE TABLE IF NOT EXISTS "location_area_prose" (
	"location_area_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	PRIMARY KEY("location_area_id","local_language_id"),
	FOREIGN KEY("location_area_id") REFERENCES "location_areas"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "ability_changelog" (
	"id"	INTEGER NOT NULL,
	"ability_id"	INTEGER NOT NULL,
	"changed_in_version_group_id"	INTEGER NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("ability_id") REFERENCES "abilities"("id"),
	FOREIGN KEY("changed_in_version_group_id") REFERENCES "version_groups"("id")
);
CREATE TABLE IF NOT EXISTS "nature_battle_style_preferences" (
	"nature_id"	INTEGER NOT NULL,
	"move_battle_style_id"	INTEGER NOT NULL,
	"low_hp_preference"	INTEGER NOT NULL,
	"high_hp_preference"	INTEGER NOT NULL,
	PRIMARY KEY("nature_id","move_battle_style_id"),
	FOREIGN KEY("nature_id") REFERENCES "natures"("id"),
	FOREIGN KEY("move_battle_style_id") REFERENCES "move_battle_styles"("id")
);
CREATE TABLE IF NOT EXISTS "location_areas" (
	"id"	INTEGER NOT NULL,
	"location_id"	INTEGER NOT NULL,
	"game_index"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79),
	PRIMARY KEY("id"),
	FOREIGN KEY("location_id") REFERENCES "locations"("id")
);
CREATE TABLE IF NOT EXISTS "encounter_condition_value_prose" (
	"encounter_condition_value_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("encounter_condition_value_id","local_language_id"),
	FOREIGN KEY("encounter_condition_value_id") REFERENCES "encounter_condition_values"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "location_game_indices" (
	"location_id"	INTEGER NOT NULL,
	"generation_id"	INTEGER NOT NULL,
	"game_index"	INTEGER NOT NULL,
	PRIMARY KEY("location_id","generation_id","game_index"),
	FOREIGN KEY("location_id") REFERENCES "locations"("id"),
	FOREIGN KEY("generation_id") REFERENCES "generations"("id")
);
CREATE TABLE IF NOT EXISTS "characteristics" (
	"id"	INTEGER NOT NULL,
	"stat_id"	INTEGER NOT NULL,
	"gene_mod_5"	INTEGER NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("stat_id") REFERENCES "stats"("id")
);
CREATE TABLE IF NOT EXISTS "natures" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"decreased_stat_id"	INTEGER NOT NULL,
	"increased_stat_id"	INTEGER NOT NULL,
	"hates_flavor_id"	INTEGER NOT NULL,
	"likes_flavor_id"	INTEGER NOT NULL,
	"game_index"	INTEGER NOT NULL UNIQUE,
	PRIMARY KEY("id"),
	FOREIGN KEY("decreased_stat_id") REFERENCES "stats"("id"),
	FOREIGN KEY("likes_flavor_id") REFERENCES "contest_types"("id"),
	FOREIGN KEY("increased_stat_id") REFERENCES "stats"("id"),
	FOREIGN KEY("hates_flavor_id") REFERENCES "contest_types"("id")
);
CREATE TABLE IF NOT EXISTS "version_groups" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL UNIQUE,
	"generation_id"	INTEGER NOT NULL,
	"order"	INTEGER,
	PRIMARY KEY("id"),
	FOREIGN KEY("generation_id") REFERENCES "generations"("id")
);
CREATE TABLE IF NOT EXISTS "item_category_prose" (
	"item_category_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("item_category_id","local_language_id"),
	FOREIGN KEY("item_category_id") REFERENCES "item_categories"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "items" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"category_id"	INTEGER NOT NULL,
	"cost"	INTEGER NOT NULL,
	"fling_power"	INTEGER,
	"fling_effect_id"	INTEGER,
	PRIMARY KEY("id"),
	FOREIGN KEY("category_id") REFERENCES "item_categories"("id"),
	FOREIGN KEY("fling_effect_id") REFERENCES "item_fling_effects"("id")
);
CREATE TABLE IF NOT EXISTS "location_names" (
	"location_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("location_id","local_language_id"),
	FOREIGN KEY("location_id") REFERENCES "locations"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "stat_names" (
	"stat_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("stat_id","local_language_id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("stat_id") REFERENCES "stats"("id")
);
CREATE TABLE IF NOT EXISTS "types" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"generation_id"	INTEGER NOT NULL,
	"damage_class_id"	INTEGER,
	PRIMARY KEY("id"),
	FOREIGN KEY("damage_class_id") REFERENCES "move_damage_classes"("id"),
	FOREIGN KEY("generation_id") REFERENCES "generations"("id")
);
CREATE TABLE IF NOT EXISTS "abilities" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"generation_id"	INTEGER NOT NULL,
	"is_main_series"	BOOLEAN NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("generation_id") REFERENCES "generations"("id"),
	CHECK(is_main_series IN (0,1))
);
CREATE TABLE IF NOT EXISTS "pokedex_prose" (
	"pokedex_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	"description"	TEXT,
	PRIMARY KEY("pokedex_id","local_language_id"),
	FOREIGN KEY("pokedex_id") REFERENCES "pokedexes"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "generation_names" (
	"generation_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("generation_id","local_language_id"),
	FOREIGN KEY("generation_id") REFERENCES "generations"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "encounter_method_prose" (
	"encounter_method_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	FOREIGN KEY("encounter_method_id") REFERENCES "encounter_methods"("id"),
	PRIMARY KEY("encounter_method_id","local_language_id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "pokeathlon_stat_names" (
	"pokeathlon_stat_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("pokeathlon_stat_id","local_language_id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("pokeathlon_stat_id") REFERENCES "pokeathlon_stats"("id")
);
CREATE TABLE IF NOT EXISTS "pokedexes" (
	"id"	INTEGER NOT NULL,
	"region_id"	INTEGER,
	"identifier"	VARCHAR(79) NOT NULL,
	"is_main_series"	BOOLEAN NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("region_id") REFERENCES "regions"("id"),
	CHECK(is_main_series IN (0,1))
);
CREATE TABLE IF NOT EXISTS "contest_effect_prose" (
	"contest_effect_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"flavor_text"	TEXT,
	"effect"	TEXT,
	PRIMARY KEY("contest_effect_id","local_language_id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("contest_effect_id") REFERENCES "contest_effects"("id")
);
CREATE TABLE IF NOT EXISTS "item_pocket_names" (
	"item_pocket_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("item_pocket_id","local_language_id"),
	FOREIGN KEY("item_pocket_id") REFERENCES "item_pockets"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "move_meta_ailment_names" (
	"move_meta_ailment_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("move_meta_ailment_id","local_language_id"),
	FOREIGN KEY("move_meta_ailment_id") REFERENCES "move_meta_ailments"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "locations" (
	"id"	INTEGER NOT NULL,
	"region_id"	INTEGER,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("region_id") REFERENCES "regions"("id")
);
CREATE TABLE IF NOT EXISTS "egg_group_prose" (
	"egg_group_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("egg_group_id","local_language_id"),
	FOREIGN KEY("egg_group_id") REFERENCES "egg_groups"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "item_categories" (
	"id"	INTEGER NOT NULL,
	"pocket_id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("pocket_id") REFERENCES "item_pockets"("id")
);
CREATE TABLE IF NOT EXISTS "move_effect_prose" (
	"move_effect_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"short_effect"	TEXT,
	"effect"	TEXT,
	PRIMARY KEY("move_effect_id","local_language_id"),
	FOREIGN KEY("move_effect_id") REFERENCES "move_effects"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_habitat_names" (
	"pokemon_habitat_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("pokemon_habitat_id","local_language_id"),
	FOREIGN KEY("pokemon_habitat_id") REFERENCES "pokemon_habitats"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "move_flag_prose" (
	"move_flag_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	"description"	TEXT,
	PRIMARY KEY("move_flag_id","local_language_id"),
	FOREIGN KEY("move_flag_id") REFERENCES "move_flags"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "move_battle_style_prose" (
	"move_battle_style_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("move_battle_style_id","local_language_id"),
	FOREIGN KEY("move_battle_style_id") REFERENCES "move_battle_styles"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "item_flag_prose" (
	"item_flag_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	"description"	TEXT,
	PRIMARY KEY("item_flag_id","local_language_id"),
	FOREIGN KEY("item_flag_id") REFERENCES "item_flags"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "pal_park_area_names" (
	"pal_park_area_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("pal_park_area_id","local_language_id"),
	FOREIGN KEY("pal_park_area_id") REFERENCES "pal_park_areas"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "contest_type_names" (
	"contest_type_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	"flavor"	TEXT,
	"color"	TEXT,
	PRIMARY KEY("contest_type_id","local_language_id"),
	FOREIGN KEY("contest_type_id") REFERENCES "contest_types"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "stats" (
	"id"	INTEGER NOT NULL,
	"damage_class_id"	INTEGER,
	"identifier"	VARCHAR(79) NOT NULL,
	"is_battle_only"	BOOLEAN NOT NULL,
	"game_index"	INTEGER,
	PRIMARY KEY("id"),
	FOREIGN KEY("damage_class_id") REFERENCES "move_damage_classes"("id"),
	CHECK(is_battle_only IN (0,1))
);
CREATE TABLE IF NOT EXISTS "move_damage_class_prose" (
	"move_damage_class_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	"description"	TEXT,
	PRIMARY KEY("move_damage_class_id","local_language_id"),
	FOREIGN KEY("move_damage_class_id") REFERENCES "move_damage_classes"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "super_contest_effect_prose" (
	"super_contest_effect_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"flavor_text"	TEXT NOT NULL,
	PRIMARY KEY("super_contest_effect_id","local_language_id"),
	FOREIGN KEY("super_contest_effect_id") REFERENCES "super_contest_effects"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "growth_rate_prose" (
	"growth_rate_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("growth_rate_id","local_language_id"),
	FOREIGN KEY("growth_rate_id") REFERENCES "growth_rates"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "evolution_trigger_prose" (
	"evolution_trigger_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("evolution_trigger_id","local_language_id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("evolution_trigger_id") REFERENCES "evolution_triggers"("id")
);
CREATE TABLE IF NOT EXISTS "encounter_condition_prose" (
	"encounter_condition_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	FOREIGN KEY("encounter_condition_id") REFERENCES "encounter_conditions"("id"),
	PRIMARY KEY("encounter_condition_id","local_language_id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "generations" (
	"id"	INTEGER NOT NULL,
	"main_region_id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id"),
	FOREIGN KEY("main_region_id") REFERENCES "regions"("id")
);
CREATE TABLE IF NOT EXISTS "language_names" (
	"language_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("language_id","local_language_id"),
	FOREIGN KEY("language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "berry_firmness_names" (
	"berry_firmness_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("berry_firmness_id","local_language_id"),
	FOREIGN KEY("berry_firmness_id") REFERENCES "berry_firmness"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "encounter_condition_values" (
	"id"	INTEGER NOT NULL,
	"encounter_condition_id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"is_default"	BOOLEAN NOT NULL,
	FOREIGN KEY("encounter_condition_id") REFERENCES "encounter_conditions"("id"),
	PRIMARY KEY("id"),
	CHECK(is_default IN (0,1))
);
CREATE TABLE IF NOT EXISTS "region_names" (
	"region_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("region_id","local_language_id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id"),
	FOREIGN KEY("region_id") REFERENCES "regions"("id")
);
CREATE TABLE IF NOT EXISTS "item_fling_effect_prose" (
	"item_fling_effect_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"effect"	TEXT NOT NULL,
	PRIMARY KEY("item_fling_effect_id","local_language_id"),
	FOREIGN KEY("item_fling_effect_id") REFERENCES "item_fling_effects"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "move_target_prose" (
	"move_target_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	"description"	TEXT,
	PRIMARY KEY("move_target_id","local_language_id"),
	FOREIGN KEY("move_target_id") REFERENCES "move_targets"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_shape_prose" (
	"pokemon_shape_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	"awesome_name"	VARCHAR(79),
	"description"	TEXT,
	PRIMARY KEY("pokemon_shape_id","local_language_id"),
	FOREIGN KEY("pokemon_shape_id") REFERENCES "pokemon_shapes"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "move_meta_category_prose" (
	"move_meta_category_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"description"	TEXT NOT NULL,
	PRIMARY KEY("move_meta_category_id","local_language_id"),
	FOREIGN KEY("move_meta_category_id") REFERENCES "move_meta_categories"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_move_method_prose" (
	"pokemon_move_method_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79),
	"description"	TEXT,
	PRIMARY KEY("pokemon_move_method_id","local_language_id"),
	FOREIGN KEY("pokemon_move_method_id") REFERENCES "pokemon_move_methods"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "experience" (
	"growth_rate_id"	INTEGER NOT NULL,
	"level"	INTEGER NOT NULL,
	"experience"	INTEGER NOT NULL,
	PRIMARY KEY("growth_rate_id","level"),
	FOREIGN KEY("growth_rate_id") REFERENCES "growth_rates"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_color_names" (
	"pokemon_color_id"	INTEGER NOT NULL,
	"local_language_id"	INTEGER NOT NULL,
	"name"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("pokemon_color_id","local_language_id"),
	FOREIGN KEY("pokemon_color_id") REFERENCES "pokemon_colors"("id"),
	FOREIGN KEY("local_language_id") REFERENCES "languages"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_shapes" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "super_contest_effects" (
	"id"	INTEGER NOT NULL,
	"appeal"	SMALLINT NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_colors" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "item_flags" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "encounter_conditions" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "move_effects" (
	"id"	INTEGER NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "encounter_methods" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL UNIQUE,
	"order"	INTEGER NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "contest_effects" (
	"id"	INTEGER NOT NULL,
	"appeal"	SMALLINT NOT NULL,
	"jam"	SMALLINT NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "move_damage_classes" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "move_battle_styles" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "regions" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL UNIQUE,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "languages" (
	"id"	INTEGER NOT NULL,
	"iso639"	VARCHAR(79) NOT NULL,
	"iso3166"	VARCHAR(79) NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"official"	BOOLEAN NOT NULL,
	"order"	INTEGER,
	PRIMARY KEY("id"),
	CHECK(official IN (0,1))
);
CREATE TABLE IF NOT EXISTS "pal_park_areas" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "item_pockets" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "egg_groups" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "item_fling_effects" (
	"id"	INTEGER NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "evolution_triggers" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "growth_rates" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	"formula"	TEXT NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "genders" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "contest_types" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "move_flags" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "berry_firmness" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "move_meta_categories" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "move_meta_ailments" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_move_methods" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_habitats" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "pokeathlon_stats" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "move_targets" (
	"id"	INTEGER NOT NULL,
	"identifier"	VARCHAR(79) NOT NULL,
	PRIMARY KEY("id")
);
/* Indexing "NAMES" is a bad decision, on my part... i'm not sure why i did it... */
CREATE INDEX IF NOT EXISTS "ix_pokemon_form_names_form_name" ON "pokemon_form_names" (
	"form_name"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_form_names_pokemon_name" ON "pokemon_form_names" (
	"pokemon_name"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_moves_level" ON "pokemon_moves" (
	"level"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_moves_move_id" ON "pokemon_moves" (
	"move_id"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_moves_pokemon_id" ON "pokemon_moves" (
	"pokemon_id"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_moves_pokemon_move_method_id" ON "pokemon_moves" (
	"pokemon_move_method_id"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_moves_version_group_id" ON "pokemon_moves" (
	"version_group_id"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_abilities_is_hidden" ON "pokemon_abilities" (
	"is_hidden"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_species_names_name" ON "pokemon_species_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_is_default" ON "pokemon" (
	"is_default"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_order" ON "pokemon" (
	"order"
);
CREATE INDEX IF NOT EXISTS "ix_version_names_name" ON "version_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_stat_changes_change" ON "move_meta_stat_changes" (
	"change"
);
CREATE INDEX IF NOT EXISTS "ix_move_names_name" ON "move_names" (
	"name"
);
--CREATE INDEX IF NOT EXISTS "ix_pokemon_species_conquest_order" ON "pokemon_species" (
--	"conquest_order"
--);
CREATE INDEX IF NOT EXISTS "ix_pokemon_species_order" ON "pokemon_species" (
	"order"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_stat_chance" ON "move_meta" (
	"stat_chance"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_crit_rate" ON "move_meta" (
	"crit_rate"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_min_hits" ON "move_meta" (
	"min_hits"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_max_turns" ON "move_meta" (
	"max_turns"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_ailment_chance" ON "move_meta" (
	"ailment_chance"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_drain" ON "move_meta" (
	"drain"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_max_hits" ON "move_meta" (
	"max_hits"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_flinch_chance" ON "move_meta" (
	"flinch_chance"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_healing" ON "move_meta" (
	"healing"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_min_turns" ON "move_meta" (
	"min_turns"
);
CREATE INDEX IF NOT EXISTS "ix_characteristic_text_message" ON "characteristic_text" (
	"message"
);
CREATE INDEX IF NOT EXISTS "ix_item_names_name" ON "item_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_type_names_name" ON "type_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_ability_names_name" ON "ability_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_nature_names_name" ON "nature_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_location_area_prose_name" ON "location_area_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_encounter_condition_value_prose_name" ON "encounter_condition_value_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_characteristics_gene_mod_5" ON "characteristics" (
	"gene_mod_5"
);
CREATE INDEX IF NOT EXISTS "ix_item_category_prose_name" ON "item_category_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_location_names_name" ON "location_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_stat_names_name" ON "stat_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_abilities_is_main_series" ON "abilities" (
	"is_main_series"
);
CREATE INDEX IF NOT EXISTS "ix_pokedex_prose_name" ON "pokedex_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_generation_names_name" ON "generation_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_encounter_method_prose_name" ON "encounter_method_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_pokeathlon_stat_names_name" ON "pokeathlon_stat_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_item_pocket_names_name" ON "item_pocket_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_move_meta_ailment_names_name" ON "move_meta_ailment_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_egg_group_prose_name" ON "egg_group_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_habitat_names_name" ON "pokemon_habitat_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_move_flag_prose_name" ON "move_flag_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_move_battle_style_prose_name" ON "move_battle_style_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_item_flag_prose_name" ON "item_flag_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_pal_park_area_names_name" ON "pal_park_area_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_contest_type_names_name" ON "contest_type_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_move_damage_class_prose_name" ON "move_damage_class_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_growth_rate_prose_name" ON "growth_rate_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_evolution_trigger_prose_name" ON "evolution_trigger_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_encounters_id" ON "encounters" (
	"id"
);
CREATE INDEX IF NOT EXISTS "ix_encounter_condition_prose_name" ON "encounter_condition_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_language_names_name" ON "language_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_berry_firmness_names_name" ON "berry_firmness_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_region_names_name" ON "region_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_move_target_prose_name" ON "move_target_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_shape_prose_name" ON "pokemon_shape_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_move_method_prose_name" ON "pokemon_move_method_prose" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_pokemon_color_names_name" ON "pokemon_color_names" (
	"name"
);
CREATE INDEX IF NOT EXISTS "ix_languages_official" ON "languages" (
	"official"
);
CREATE UNIQUE INDEX IF NOT EXISTS "ix_move_meta_categories_identifier" ON "move_meta_categories" (
	"identifier"
);
CREATE UNIQUE INDEX IF NOT EXISTS "ix_move_meta_ailments_identifier" ON "move_meta_ailments" (
	"identifier"
);
COMMIT;
--
BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "game_player" (
	"id"	INTEGER NOT NULL, --Game Slot
	"ChallengeId"	INTEGER NOT NULL, 
	"PlayerMoney"	INTEGER NOT NULL,
	"PlayerCoins"	INTEGER NOT NULL,
	"PlayerSavings"	INTEGER NOT NULL,
	"IsCreator"	BIT NOT NULL,
	"StorageOwnerName"	NVARCHAR(18) NOT NULL,
	"ActiveStorageBox"	INTEGER NOT NULL,
	"TrainerName"	NVARCHAR(18) NOT NULL,
	"TrainerIsMale"	BIT NOT NULL,
	"TrainerPublicId"	INTEGER NOT NULL,
	"TrainerSecretId"	INTEGER NOT NULL,
	"TimeCreated"	DATETIME NOT NULL,
	"PlayTime"	INTEGER NOT NULL, --TimeSpan to Int
	"RandomSeed"	INTEGER NOT NULL, --Seed for RNG... even if players reset, rolling dice yields same results
	--PRIMARY KEY("id"),
	PRIMARY KEY("TrainerPublicId","TrainerSecretId")
);
CREATE TABLE IF NOT EXISTS "game_player_pokedex" (
	"game_id"	INTEGER NOT NULL,
	--"TrainerPublicId"	INTEGER NOT NULL,
	--"TrainerSecretId"	INTEGER NOT NULL,
	"pokemon_species_id"	INTEGER NOT NULL, --"Species"
	"pokemon_form_id"	INTEGER NOT NULL, --"Form"
	"HasCaptured"	BIT NOT NULL, 
	--PRIMARY KEY("TrainerSecretId","TrainerPublicId","pokemon_species_id","pokemon_form_id"),
	PRIMARY KEY("game_id","pokemon_species_id","pokemon_form_id"),
	FOREIGN KEY("pokemon_species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("pokemon_form_id") REFERENCES "pokemon_forms"("id"),
	--FOREIGN KEY("TrainerPublicId") REFERENCES "game_player"("TrainerPublicId"),
	--FOREIGN KEY("TrainerSecretId") REFERENCES "game_player"("TrainerSecretId")
	FOREIGN KEY("game_id") REFERENCES "game_player"("id")
);
CREATE TABLE IF NOT EXISTS "game_player_pokemon_storage" (
	"game_id"	INTEGER NOT NULL,
	"slot_id"	INTEGER NOT NULL, --box
	"slot_background_id"	INTEGER NOT NULL, --theme/texture
	"slot_name"	NVARCHAR(18) NOT NULL, --label
	"unlocked"	BIT NOT NULL, 
	PRIMARY KEY("game_id","slot_id"),
	FOREIGN KEY("game_id") REFERENCES "game_player"("id")
);
CREATE TABLE IF NOT EXISTS "game_player_item_storage" (
	"game_id"	INTEGER NOT NULL,
	"item_location_id"	BIT NOT NULL, --bag / pc?
	--"slot_id"	INTEGER NOT NULL, --box / order?
	"item_id"	INTEGER NOT NULL, --item
	"quantity"	INTEGER NOT NULL, --amount
	"limit"		INTEGER NULL, --is this needed?
	PRIMARY KEY("game_id","item_location_id","slot_id"),
	FOREIGN KEY("game_id") REFERENCES "game_player"("id")
	FOREIGN KEY("item_id") REFERENCES "items"("id")
);
/*CREATE TABLE IF NOT EXISTS "game_player_bag" ( --merge with item_storage?
	"game_id"	INTEGER NOT NULL,
	--"slot_id"	INTEGER NOT NULL, --pocket
	"item_id"	INTEGER NOT NULL, --item
	"quantity"	INTEGER NOT NULL, --amount
	--"limit"		INTEGER NULL, --is this needed?
	PRIMARY KEY("game_id","item_id"),
	FOREIGN KEY("game_id") REFERENCES "game_player"("id")
);*/
CREATE TABLE IF NOT EXISTS "game_player_pokemon" (
	"id"	INTEGER NOT NULL, --"PersonalID" no duplicates
	"NickName"	nvarchar(14) NULL, 
	"pokemon_form_id"	INTEGER NOT NULL, --"Form"
	"pokemon_species_id"	INTEGER NOT NULL, --"Species"
	"ability_id"	INTEGER NOT NULL, --"Ability"
	"nature_id"	INTEGER NOT NULL, --"Nature"
	"IsShiny"	BIT NOT NULL,
	"Gender"	BIT NULL,
	"PokerusStatus"	INTEGER NOT NULL,
	"PokerusDuration"	INTEGER NOT NULL,
	"ShadowLevel"	INTEGER NULL,
	"HeartGuageSize"	INTEGER NOT NULL,
	"CurrentHP"	INTEGER NOT NULL,
	"held_item_id"	INTEGER NOT NULL, --"Item"
	"IV_hp"		INTEGER NOT NULL,
	"IV_atk"	INTEGER NOT NULL,
	"IV_def"	INTEGER NOT NULL,
	"IV_spa"	INTEGER NOT NULL,
	"IV_spd"	INTEGER NOT NULL,
	"IV_spe"	INTEGER NOT NULL,
	"EV"	NVARCHAR(8) NOT NULL, --Array[6]
	"ObtainedLevel"	INTEGER NOT NULL,
	"TotalExp"	INTEGER NOT NULL,
	"Happiness"	INTEGER NOT NULL,
	"Status"	INTEGER NOT NULL,
	"StatusCount"	INTEGER NOT NULL,
	"EggSteps"	INTEGER NOT NULL,
	"BallUsed_item_id"	INTEGER NOT NULL, --"BallUsed"
	"Mail"	NVARCHAR(100) NULL, --"Message"
	"TrainerName"	NVARCHAR(18) NOT NULL,
	"TrainerIsMale"	BIT NOT NULL,
	"TrainerPublicId"	INTEGER NOT NULL,
	"TrainerSecretId"	INTEGER NOT NULL,
	"ObtainedMethod"	INTEGER NOT NULL,
	"TimeReceived"	DATETIME NOT NULL,
	"TimeEggHatched"	DATETIME NOT NULL,
	"Markings1"	BIT NOT NULL,
	"Markings2"	BIT NOT NULL,
	"Markings3"	BIT NOT NULL,
	"Markings4"	BIT NOT NULL,
	"Markings5"	BIT NOT NULL,
	"Markings6"	BIT NOT NULL,
	PRIMARY KEY("id"), 
	--FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id"),
	--FOREIGN KEY("pokemon_species_id") REFERENCES "pokemon_species"("id"),
	--should FK pokedex, to keep hack tampering low
	FOREIGN KEY("pokemon_species_id") REFERENCES "game_player_pokedex"("pokemon_species_id"), --dont register pokemons to save if pokedex hasnt documented it
	FOREIGN KEY("pokemon_form_id") REFERENCES "pokemon_forms"("id"),
	FOREIGN KEY("ability_id") REFERENCES "abilities"("id"),
	FOREIGN KEY("nature_id") REFERENCES "natures"("id"),
	FOREIGN KEY("held_item_id") REFERENCES "items"("id"),
	FOREIGN KEY("BallUsed_item_id") REFERENCES "items"("id")
);
CREATE TABLE IF NOT EXISTS "game_player_pokemon_location" (
	"pokemon_personal_id"	INTEGER NOT NULL UNIQUE ON CONFLICT REPLACE, --no duplicates
	"pokemon_game_id"	INTEGER NOT NULL, --game state
	"pokemon_location_id"	BIT NULL, --party / pc, or wild... make int and add `game modes` (stadium saved party)?
	"pokemon_slot_position"	INTEGER NOT NULL, --where
	PRIMARY KEY("pokemon_game_id", "pokemon_location_id","pokemon_slot_position"), --one slot per pokemon
	FOREIGN KEY("pokemon_personal_id") REFERENCES "game_player_pokemon"("id")
);
CREATE TABLE IF NOT EXISTS "game_player_pokemon_move_archive" (
	"pokemon_game_id"	INTEGER NOT NULL, --just in case pokemon is duplicate across games?
	"pokemon_personal_id"	INTEGER NOT NULL,
	"move_id"	INTEGER NOT NULL,
	PRIMARY KEY("pokemon_game_id","pokemon_personal_id","move_id"), 
	FOREIGN KEY("pokemon_personal_id") REFERENCES "game_player_pokemon"("id"),
	FOREIGN KEY("move_id") REFERENCES "moves"("id")
);
CREATE TABLE IF NOT EXISTS "game_player_pokemon_moves" (
	"pokemon_game_id"	INTEGER NOT NULL, --just in case pokemon is duplicate across games?
	"pokemon_personal_id"	INTEGER NOT NULL, 
	"move_slot_position"	INTEGER NOT NULL,
	"move_id"	INTEGER NOT NULL,
	"PP"	INTEGER NOT NULL,
	"PPups"	INTEGER NOT NULL,
	PRIMARY KEY("pokemon_game_id","pokemon_personal_id","move_id"), 
	FOREIGN KEY("pokemon_personal_id") REFERENCES "game_player_pokemon"("id"),
	FOREIGN KEY("move_id") REFERENCES "game_player_pokemon_move_archive"("move_id") --Adds more security to ensure no cheating
);
CREATE TABLE IF NOT EXISTS "game_player_pokemon_ribbon" (
	"pokemon_game_id"	INTEGER NOT NULL, --just in case pokemon is duplicate across games?
	"pokemon_personal_id"	INTEGER NOT NULL,
	"ribbon_id"	INTEGER NOT NULL,
	PRIMARY KEY("pokemon_game_id","pokemon_personal_id","ribbon_id"),
	FOREIGN KEY("pokemon_personal_id") REFERENCES "game_player_pokemon"("id")
	--,FOREIGN KEY("ribbon_id") REFERENCES "ribbons"("id")
);
CREATE TABLE IF NOT EXISTS "pokemon_ui_sprite" (
	"id"	NVARCHAR(15) NOT NULL, --name of the sprite image
	"pokemon_species_id"	INTEGER NOT NULL, --"Species"
	"pokemon_form_id"	INTEGER NOT NULL, --"Form"
	"IsShiny"	BIT NOT NULL,
	"Gender"	BIT NULL, --gender this sprite is specific to, null if both use same sprite
	PRIMARY KEY("id"), 
	--FOREIGN KEY("pokemon_id") REFERENCES "pokemon"("id"),
	FOREIGN KEY("pokemon_species_id") REFERENCES "pokemon_species"("id"),
	FOREIGN KEY("pokemon_form_id") REFERENCES "pokemon_forms"("id")
);
CREATE TABLE IF NOT EXISTS "item_ui_sprite" (
	"id"		NVARCHAR(15) NOT NULL, --name of the sprite image
	"item_id"	INTEGER NULL, --"item"
	"filepath"	NVARCHAR(75) NOT NULL, --name with folder structure
	"IsCopy"	BIT NOT NULL, --duplicate of same item used somewhere else
	"notes"		NVARCHAR(75) NOT NULL, --name with folder structure
	"reuse_item_id"	NVARCHAR(75) NOT NULL, --the id of item that uses this exact sprite, to prevent duplicates
	PRIMARY KEY("id"), 
	FOREIGN KEY("item_id") REFERENCES "items"("id")
);
COMMIT;
--
BEGIN TRANSACTION;
CREATE VIEW pokemon_abilities_view as
select 
	p.id as pokemon_id,
	CAST(AVG(CASE WHEN a.slot = 1 THEN a.ability_id
	END) as int) as ability1
	,CAST(AVG(CASE WHEN a.slot = 2 THEN a.ability_id 
	END) as int) as ability2
	,CAST(AVG(CASE WHEN a.slot = 3 THEN a.ability_id 
	END) as int) as ability3
	from "pokemon" as p
	left join "pokemon_abilities" as a on p.id = a.pokemon_id
	group by p.id;
CREATE VIEW pokemon_stats_view as
select 
	p.id as pokemon_id
	,CAST(AVG(CASE WHEN i.stat_id = 1 THEN i.base_stat
	END) as int) as bhp
	,CAST(AVG(CASE WHEN i.stat_id = 2 THEN i.base_stat
	END) as int) as batk
	,CAST(AVG(CASE WHEN i.stat_id = 3 THEN i.base_stat
	END) as int) as bdef
	,CAST(AVG(CASE WHEN i.stat_id = 4 THEN i.base_stat
	END) as int) as bspa
	,CAST(AVG(CASE WHEN i.stat_id = 5 THEN i.base_stat
	END) as int) as bspd
	,CAST(AVG(CASE WHEN i.stat_id = 6 THEN i.base_stat
	END) as int) as bspe
	,CAST(AVG(CASE WHEN i.stat_id = 1 THEN i.effort
	END) as int) as ehp
	,CAST(AVG(CASE WHEN i.stat_id = 2 THEN i.effort
	END) as int) as eatk
	,CAST(AVG(CASE WHEN i.stat_id = 3 THEN i.effort
	END) as int) as edef
	,CAST(AVG(CASE WHEN i.stat_id = 4 THEN i.effort
	END) as int) as espa
	,CAST(AVG(CASE WHEN i.stat_id = 5 THEN i.effort
	END) as int) as espd
	,CAST(AVG(CASE WHEN i.stat_id = 6 THEN i.effort
	END) as int) as espe
	from "pokemon" as p
	left join "pokemon_stats" as i on p.id = i.pokemon_id
	group by p.id;
CREATE VIEW pokemon_egg_groups_view as
select 
	p.id as pokemon_id,
	MIN(e.egg_group_id) as egg_group1
	,CASE WHEN COUNT(e.species_id) = 2 THEN MAX(e.egg_group_id) --ELSE 0   
	END as egg_group2
	from "pokemon" as p
	left join "pokemon_egg_groups" as e on p.species_id = e.species_id
	group by p.id;
CREATE VIEW pokemon_types_view as
select 
	p.id as pokemon_id,
	CAST(AVG(CASE WHEN t.slot = 1 THEN t.type_id  
	END) as int) as type1
	,CAST(AVG(CASE WHEN t.slot = 2 THEN t.type_id --ELSE 0
	END) as int) as type2
	from "pokemon" as p
	left join "pokemon_types" as t on p.id = t.pokemon_id
	group by p.id;
/* Forms -> Pokemon -> Species Relationship  
CREATE VIEW pokemon_forms_view as
--select REPLACE('case Forms.' || UPPER(pokemon_forms.identifier) || ':
--	return Pokemons.' || UPPER (pokemon.identifier) || ';', '-', '_') as Switch,
pokemon_forms.id, pokemon_forms.identifier,
pokemon.id, pokemon.species_id, pokemon.identifier, pokemon.height, pokemon.weight, pokemon.base_experience, --pokemon."order"
pokemon_stats_view.bhp, pokemon_stats_view.batk, pokemon_stats_view.bdef, pokemon_stats_view.bspa, pokemon_stats_view.bspd, pokemon_stats_view.bspe, pokemon_stats_view.ehp, pokemon_stats_view.eatk, pokemon_stats_view.edef, pokemon_stats_view.espa, pokemon_stats_view.espd, pokemon_stats_view.espe,
pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.color_id, pokemon_species.shape_id, pokemon_species.habitat_id, pokemon_species.gender_rate, pokemon_species.capture_rate, pokemon_species.base_happiness, pokemon_species.is_baby, pokemon_species.hatch_counter, pokemon_species.has_gender_differences, pokemon_species.growth_rate_id, pokemon_species.forms_switchable, pokemon_species."order"
from pokemon
left join pokemon_forms on pokemon_forms.pokemon_id = pokemon.id
left join pokemon_form_names on pokemon_form_names.pokemon_form_id = pokemon_forms.id AND pokemon_form_names.local_language_id=9
left join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id 
left join pokemon_species on pokemon_species.id = pokemon.species_id
left join pokemon_species_names on pokemon_species_names.pokemon_species_id = pokemon.species_id AND pokemon_species_names.local_language_id=9
where pokemon_species.id != pokemon.id AND
--pokemon_forms.identifier != pokemon.identifier 
pokemon_forms.id != pokemon.id
--pokemon_forms.id != pokemon_species.id
order by pokemon.species_id, pokemon_forms.id ASC;*/
CREATE VIEW pokemon_views as 
select pokemon.id, pokemon.species_id, pokemon.identifier, pokemon.height, pokemon.weight, pokemon.base_experience, --pokemon."order"
pokemon_abilities_view.ability1, pokemon_abilities_view.ability2, pokemon_abilities_view.ability3, 
pokemon_egg_groups_view.egg_group1, pokemon_egg_groups_view.egg_group2,
pokemon_stats_view.bhp, pokemon_stats_view.batk, pokemon_stats_view.bdef, pokemon_stats_view.bspa, pokemon_stats_view.bspd, pokemon_stats_view.bspe, pokemon_stats_view.ehp, pokemon_stats_view.eatk, pokemon_stats_view.edef, pokemon_stats_view.espa, pokemon_stats_view.espd, pokemon_stats_view.espe,
pokemon_types_view.type1, pokemon_types_view.type2,
pokemon_color_names.name as color,
pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.color_id, pokemon_species.shape_id, pokemon_species.habitat_id, pokemon_species.gender_rate, pokemon_species.capture_rate, pokemon_species.base_happiness, pokemon_species.is_baby, pokemon_species.hatch_counter, pokemon_species.has_gender_differences, pokemon_species.growth_rate_id, pokemon_species.forms_switchable, pokemon_species."order",
evolution_chains.baby_trigger_item_id as incense,
ifnull(pokemon_form_names.pokemon_name, pokemon_species_names.name) as name,pokemon_species_names.genus,
pokemon_species_flavor_text.flavor_text
from pokemon
left join pokemon_abilities_view on pokemon.id = pokemon_abilities_view.pokemon_id 
left join pokemon_egg_groups_view on pokemon_egg_groups_view.pokemon_id = pokemon.id 
left join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id 
left join pokemon_types_view on pokemon_types_view.pokemon_id = pokemon.id 
left join pokemon_species on pokemon_species.id = pokemon.species_id
--left join pokemon_evolution on pokemon_evolution.species_id = pokemon_species.id
left join evolution_chains on evolution_chains.id = pokemon_species.evolution_chain_id
left join pokemon_colors on pokemon_colors.id = pokemon_species.color_id
left join pokemon_color_names on pokemon_color_names.pokemon_color_id=pokemon_colors.id AND pokemon_color_names.local_language_id=9
left join pokemon_forms on pokemon_forms.pokemon_id = pokemon.id
left join pokemon_form_names on pokemon_form_names.pokemon_form_id = pokemon_forms.id AND pokemon_form_names.local_language_id=9
left join pokemon_species_names on pokemon_species_names.pokemon_species_id = pokemon.species_id AND pokemon_species_names.local_language_id=9
left join pokemon_species_flavor_text on pokemon_species_flavor_text.species_id = pokemon.species_id AND pokemon_species_flavor_text.version_id=26 AND pokemon_species_flavor_text.language_id=9
order by pokemon.id ASC;
CREATE VIEW pokemon_evolution_view as 
select pokemon_species.id, pokemon_species.identifier, pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species."order",
pokemon_evolution.evolved_species_id, pokemon_evolution.evolution_trigger_id, pokemon_evolution.trigger_item_id, pokemon_evolution.minimum_level, pokemon_evolution.gender_id, pokemon_evolution.location_id, pokemon_evolution.held_item_id, pokemon_evolution.time_of_day, pokemon_evolution.known_move_id, pokemon_evolution.known_move_type_id, pokemon_evolution.minimum_happiness, pokemon_evolution.minimum_beauty, pokemon_evolution.minimum_affection, pokemon_evolution.relative_physical_stats, pokemon_evolution.party_species_id, pokemon_evolution.party_type_id, pokemon_evolution.trade_species_id, pokemon_evolution.needs_overworld_rain, pokemon_evolution.turn_upside_down
from pokemon_evolution
left join pokemon_species on pokemon_evolution.evolved_species_id = pokemon_species.id
--group by pokemon_species."order" --pokemon_species.id --pokemon_species.evolution_chain_id
order by pokemon_species.evolution_chain_id, pokemon_species.id; --ToDo: Foreign/Index key "pokemon_species.evolves_from_species_id"
--pokemon_species.evolves_from_species_id, --number order is bad, went 1, 10, 100, instead of 1,2,3
pokemon_species."order"; 
CREATE VIEW item_game_indices_view as 
select item_id, group_concat(DISTINCT generation_id) as generation_group, group_concat(DISTINCT game_index) as game_index_group
from item_game_indices 
group by item_id;
CREATE VIEW item_flag_map_view as 
select item_id, group_concat(DISTINCT item_flag_id) as item_flag_group
from item_flag_map 
group by item_id;
CREATE VIEW item_views as 
select items.id, items.identifier, items.category_id, items.cost, items.fling_power, items.fling_effect_id,
item_flag_map_view.item_flag_group,
item_game_indices_view.generation_group, item_game_indices_view.game_index_group,
item_prose.short_effect, item_prose.effect,
item_categories.pocket_id,
item_pockets.identifier as item_pocket_identifier,
item_names.name,
item_flavor_text.flavor_text
from items
left join (
	select item_id, group_concat(DISTINCT item_flag_id) as item_flag_group
	from item_flag_map 
	group by item_id
	) as item_flag_map_view on item_flag_map_view.item_id = items.id 
left join (
	select item_id, group_concat(DISTINCT generation_id) as generation_group, group_concat(DISTINCT game_index) as game_index_group
	from item_game_indices 
	group by item_id
	) as item_game_indices_view on item_game_indices_view.item_id = items.id
left join item_categories on item_categories.id = items.category_id
left join item_pockets on item_pockets.id = item_categories.pocket_id
left join item_prose on item_prose.item_id=items.id AND item_prose.local_language_id=9
left join item_fling_effect_prose on item_fling_effect_prose.item_fling_effect_id = items.fling_effect_id AND item_fling_effect_prose.local_language_id=9
left join item_names on item_names.item_id = items.id AND item_names.local_language_id=9
left join item_flavor_text on item_flavor_text.item_id = items.id AND item_flavor_text.version_group_id=18 AND item_flavor_text.language_id=9
order by items.id ASC;
CREATE VIEW move_flag_map_view as 
select move_id, group_concat(DISTINCT move_flag_id) as move_flag_group
from move_flag_map 
group by move_id;
CREATE VIEW move_views as 
select moves.id, moves.identifier, moves.generation_id, moves.type_id, moves.power, moves.pp, moves.accuracy, moves.priority, moves.target_id, moves.damage_class_id, moves.effect_id, moves.effect_chance, moves.contest_type_id, moves.contest_effect_id, moves.contest_type_id, moves.contest_effect_id, moves.super_contest_effect_id,
contest_effects.appeal, contest_effects.jam, super_contest_effects.appeal as super_appeal,
move_meta.meta_category_id, move_meta.meta_ailment_id, move_meta.min_hits, move_meta.max_hits, move_meta.min_turns, move_meta.max_turns, move_meta.drain, move_meta.healing, move_meta.crit_rate, move_meta.ailment_chance, move_meta.flinch_chance, move_meta.stat_chance,
--move_flags.identifier as move_flag,
move_flag_map_view.move_flag_group,
move_effect_prose.short_effect, move_effect_prose.effect,
--move_flag_prose.name as flag_name, move_flag_prose.description as flag_description,
move_targets.identifier as target_identifier,
move_target_prose.name as target_name, move_target_prose.description as target_description,
move_names.name,
move_flavor_text.flavor_text
from moves
left join move_flag_map_view on move_flag_map_view.move_id = moves.id 
--left join move_flags on move_flags.id = moves.id 
left join move_meta on move_meta.move_id = moves.id
left join contest_effects on moves.contest_effect_id = contest_effects.id
left join super_contest_effects on super_contest_effects.id = moves.super_contest_effect_id
left join move_targets on move_targets.id = moves.target_id
left join move_target_prose on move_target_prose.move_target_id=move_targets.id AND move_target_prose.local_language_id=9
left join move_effect_prose on move_effect_prose.move_effect_id = moves.effect_id AND move_effect_prose.local_language_id=9
left join move_names on move_names.move_id = moves.id AND move_names.local_language_id=9
left join move_flavor_text on move_flavor_text.move_id = moves.id AND move_flavor_text.version_group_id=18 AND move_flavor_text.language_id=9
order by moves.id ASC;
CREATE VIEW berry_flavors_view as
select 
	b.id as berry_id
	,CAST(AVG(CASE WHEN i.contest_type_id = 1 THEN i.flavor
	END) as int) as cool
	,CAST(AVG(CASE WHEN i.contest_type_id = 2 THEN i.flavor
	END) as int) as beauty
	,CAST(AVG(CASE WHEN i.contest_type_id = 3 THEN i.flavor
	END) as int) as cute
	,CAST(AVG(CASE WHEN i.contest_type_id = 4 THEN i.flavor
	END) as int) as smart
	,CAST(AVG(CASE WHEN i.contest_type_id = 5 THEN i.flavor
	END) as int) as tough
	from "berries" as b
	left join "berry_flavors" as i on b.id = i.berry_id
	group by b.id;
CREATE VIEW berry_views as
select 
	b.id, b.item_id, b.firmness_id, b.natural_gift_power, b.natural_gift_type_id, b.size, b.max_harvest, b.growth_time, b.soil_dryness, b.smoothness
	,i.cool, i.beauty, i.cute, i.smart, i.tough
	from "berries" as b
	left join berry_flavors_view as i on b.id = i.berry_id;
CREATE VIEW contest_combos_view as 
select first_move_id, group_concat(DISTINCT second_move_id) as second_move_group
	from contest_combos 
	group by first_move_id;
CREATE VIEW encounter_condition_value_map_view as
select encounter_id, group_concat(DISTINCT encounter_condition_value_id) as encounter_condition_value_group
from encounter_condition_value_map 
group by encounter_id;
CREATE VIEW location_area_encounter_rates_view as
select location_area_id, group_concat(DISTINCT version_id) as version_group, encounter_method_id, rate
	from location_area_encounter_rates 
	group by location_area_id, encounter_method_id, rate;
CREATE VIEW location_areas_view as
select 
	l.id, l.location_id, l.identifier,
	location_area_encounter_rates_view.location_area_id, location_area_encounter_rates_view.version_group, location_area_encounter_rates_view.encounter_method_id, location_area_encounter_rates_view.rate
	from "location_areas" as l
	left join (
		select location_area_id, group_concat(DISTINCT version_id) as version_group, encounter_method_id, rate
		from location_area_encounter_rates 
		group by location_area_id, encounter_method_id, rate
	) location_area_encounter_rates_view on l.id = location_area_encounter_rates_view.location_area_id;
CREATE VIEW location_views as
select 
	l.id, l.region_id, l.identifier,
	g.generation_id,
	n.name, n.subtitle
	from "locations" as l
	left join location_game_indices as g on l.id = g.location_id
	left join location_names as n on l.id = n.location_id AND n.local_language_id=9;
CREATE VIEW encounter_views as
select 
	--e.id, e.version_id, e.location_area_id, e.encounter_slot_id, e.pokemon_id, e.min_level, e.max_level,
	e.location_area_id, group_concat(DISTINCT e.pokemon_id) as pokemon_group, MIN(e.min_level) as min_level, MAX(e.max_level) as max_level, group_concat(DISTINCT e.encounter_slot_id) as encounter_slot_group, group_concat(DISTINCT e.version_id) as version_group,
	s.encounter_method_id, s.slot, s.rarity, --group_concat(DISTINCT s.version_group_id) as encounter_slot_version_group,
	--i.encounter_condition_value_group,
	group_concat(DISTINCT i.encounter_condition_value_id) as encounter_condition_value_group,
	a.location_id,
	l.region_id,
	g.generation_id
	--group_concat(DISTINCT g.generation_id) as generation_group
	--,n.name, n.subtitle
	from "encounters" as e
	left join encounter_slots as s on s.id = e.encounter_slot_id
	--left join encounter_condition_value_map_view as i on e.id = i.encounter_id;
	--left join (
	--	select encounter_id, group_concat(DISTINCT encounter_condition_value_id) as encounter_condition_value_group
	--	from encounter_condition_value_map 
	--	group by encounter_id
	--) as i on e.id = i.encounter_id
	left join encounter_condition_value_map as i on e.id = i.encounter_id
	--left join (
	--	select location_area_id, group_concat(DISTINCT version_id) as version_group, encounter_method_id, rate
	--	from location_area_encounter_rates 
	--	group by location_area_id, encounter_method_id, rate
	--) as r on e.location_area_id = r.location_area_id
	left join location_areas as a on a.id=e.location_area_id
	left join locations as l on l.id=a.location_id
	left join location_game_indices as g on l.id = g.location_id
	left join location_names as n on l.id = n.location_id AND n.local_language_id=9
	--where i.encounter_condition_value_group IS NOT NULL
	--where i.encounter_condition_value_id IS NOT NULL
	group by e.location_area_id, s.encounter_method_id, s.slot, e.pokemon_id;
/* IGNORE THIS; JUST TESTING QUERY RESULTS
CREATE VIEW encounter_slot_views as 
select 
	--e.id, e.version_id, e.location_area_id, e.encounter_slot_id, e.pokemon_id, e.min_level, e.max_level,
	e.location_area_id, group_concat(DISTINCT e.pokemon_id) as pokemon_group, MIN(e.min_level) as min_level, MAX(e.max_level) as max_level, --group_concat(DISTINCT e.encounter_slot_id) as encounter_slot_group, group_concat(DISTINCT e.version_id) as version_group,
	s.encounter_method_id, s.slot, s.rarity, group_concat(DISTINCT s.version_group_id) as encounter_slot_version_group,
	--i.encounter_condition_value_group,
	group_concat(DISTINCT i.encounter_condition_value_id) as encounter_condition_value_group,
	a.location_id,
	l.region_id,
	g.generation_id
	--group_concat(DISTINCT g.generation_id) as generation_group
	--,n.name, n.subtitle
	from "encounters" as e
	left join encounter_slots as s on s.id = e.encounter_slot_id
	--left join encounter_condition_value_map_view as i on e.id = i.encounter_id;
	--left join (
	--	select encounter_id, group_concat(DISTINCT encounter_condition_value_id) as encounter_condition_value_group
	--	from encounter_condition_value_map 
	--	group by encounter_id
	--) as i on e.id = i.encounter_id
	left join encounter_condition_value_map as i on e.id = i.encounter_id
	--left join (
	--	select location_area_id, group_concat(DISTINCT version_id) as version_group, encounter_method_id, rate
	--	from location_area_encounter_rates 
	--	group by location_area_id, encounter_method_id, rate
	--) as r on e.location_area_id = r.location_area_id
	left join location_areas as a on a.id=e.location_area_id
	left join locations as l on l.id=a.location_id
	left join location_game_indices as g on l.id = g.location_id
	left join location_names as n on l.id = n.location_id AND n.local_language_id=9
	--where i.encounter_condition_value_group IS NOT NULL
	--where i.encounter_condition_value_id IS NOT NULL
	group by e.location_area_id, s.encounter_method_id, s.slot, s.rarity;--e.pokemon_id;*/
--CREATE VIEW encounter_slot_views as 
select encounter_method_group, slot_group, rarity_group, group_concat(DISTINCT encounter_slot_version_group) as version_group, group_concat(id_group) as id_group FROM (SELECT
	--s.id, s.encounter_method_id, s.slot, s.rarity, group_concat(DISTINCT s.version_group_id) as encounter_slot_version_group
	group_concat(s.id) as id_group, s.encounter_method_id as encounter_method_group, group_concat(DISTINCT s.slot) as slot_group, group_concat(s.rarity) as rarity_group, group_concat(DISTINCT s.version_group_id) as encounter_slot_version_group
	from ( --encounter_slots as s --on s.id = e.encounter_slot_id
		select id, version_group_id, rarity, encounter_method_id,
		case when (slot is not null) AND version_group_id = 15 then slot + 1 else slot end as slot
		from encounter_slots
	) as s group by s.version_group_id, s.encounter_method_id--s.slot, rarity
	--order by s.encounter_method_id, s.id
) group by encounter_method_group, slot_group, rarity_group 
order by encounter_method_group
COMMIT;