CREATE VIEW [VIEW].Pokemon as 
select pokemon.id, pokemon.species_id, pokemon.height, pokemon.weight, pokemon.base_experience,
abilities_view.ability1, abilities_view.ability2, abilities_view.hidden, 
egg_group_view.egg_group1, egg_group_view.egg_group2,
pokemon_stats_view.batk, pokemon_stats_view.bdef, pokemon_stats_view.bhp, pokemon_stats_view.bspa, pokemon_stats_view.bspd, pokemon_stats_view.bspe,
pokemon_type_view.type1, pokemon_type_view.type2,
pokemon_color_names.name as color,
pokemon_species.base_happiness, pokemon_species.capture_rate, pokemon_species.gender_rate, pokemon_species.hatch_counter, pokemon_species.shape_id, pokemon_species.growth_rate_id,
pokemon_species_names.name,pokemon_species_names.genus,
pokemon_species_flavor_text.flavor_text
from pokemon 
join abilities_view on pokemon.id = abilities_view.pokemon_id 
join egg_group_view on egg_group_view.species_id = pokemon.id 
join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id 
join pokemon_type_view on pokemon_type_view.pokemon_id = pokemon.id 
join pokemon_species on pokemon_species.id = pokemon.id
left join pokemon_colors on pokemon_colors.id = pokemon_species.color_id
left join pokemon_color_names on pokemon_color_names.pokemon_color_id=pokemon_colors.id AND pokemon_color_names.local_language_id=9
join pokemon_species_names on pokemon_species_names.pokemon_species_id = pokemon.id AND pokemon_species_names.local_language_id=9
join pokemon_species_flavor_text on pokemon_species_flavor_text.species_id = pokemon.id AND pokemon_species_flavor_text.version_id=26 AND pokemon_species_flavor_text.language_id=9
order by pokemon.id ASC;
GO
CREATE VIEW [VIEW].TMs as
	[machine_number]	INT,
	[version_group_id]	INT,
	[item_id]	INT,
	[move_id]	INT,
	[itemNo]	INT NOT NULL PRIMARY KEY IDENTITY,
	FOREIGN KEY([machine_number]) REFERENCES [machines]([machine_number]) ON UPDATE CASCADE ON DELETE NO ACTION,
	FOREIGN KEY([version_group_id]) REFERENCES version_group(id) ON UPDATE CASCADE ON DELETE NO ACTION,
	FOREIGN KEY([item_id]) REFERENCES [items]([id]) ON UPDATE CASCADE ON DELETE NO ACTION,
	FOREIGN KEY([move_id]) REFERENCES [moves]([id]) ON UPDATE CASCADE ON DELETE NO ACTION
);
GO
CREATE VIEW [VIEW].[PokemonType] as
	[pokemon_id]	INT NOT NULL,
	[type1]	INT,
	[type2]	INT,
	PRIMARY KEY([pokemon_id]),
	FOREIGN KEY([pokemon_id]) REFERENCES [pokemon]([id]) ON UPDATE CASCADE ON DELETE NO ACTION,
	FOREIGN KEY([type1]) REFERENCES [types]([id]) ON UPDATE CASCADE ON DELETE NO ACTION,
	FOREIGN KEY([type2]) REFERENCES [types]([id]) ON UPDATE CASCADE ON DELETE NO ACTION
);
GO
CREATE VIEW [VIEW].[PokemonStats] as
	SELECT
	[pokemon_id],	[bhp],	[batk],	[bdef],	[bspa],	[bspd],	[bspe]
	LEFT JOIN [pokemon] on [pokemon_id]=[id]
GO
CREATE VIEW [VIEW].[EggGroup] as
	SELECT 
	[species_id],	[egg_group1],	[egg_group2]
	LEFT JOIN [egg_groups] on [egg_group1] = [id]
	LEFT JOIN [egg_groups] on [egg_group2] = [id]
);