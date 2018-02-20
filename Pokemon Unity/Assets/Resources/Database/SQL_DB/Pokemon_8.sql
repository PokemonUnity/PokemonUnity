CREATE TABLE item_flavor_summaries (
	item_id INT NOT NULL, 
	local_language_id INT NOT NULL, 
	flavor_summary TEXT, 
	PRIMARY KEY (item_id, local_language_id), 
	FOREIGN KEY(item_id) REFERENCES items (id) ON UPDATE CASCADE ON DELETE NO ACTION, 
	FOREIGN KEY(local_language_id) REFERENCES languages (id) ON UPDATE CASCADE ON DELETE NO ACTION
);
--GO
--DROP TABLE IF EXISTS item_flags;
CREATE TABLE item_flags (
	id INT NOT NULL, 
	identifier VARCHAR(79) NOT NULL, 
	PRIMARY KEY (id)
);
INSERT INTO [item_flags] (id,identifier) VALUES (1,'countable'),
 (2,'consumable'),
 (3,'usable-overworld'),
 (4,'usable-in-battle'),
 (5,'holdable'),
 (6,'holdable-passive'),
 (7,'holdable-active'),
 (8,'underground');
--GO
--DROP TABLE IF EXISTS item_flag_prose;
CREATE TABLE item_flag_prose (
	item_flag_id INT NOT NULL, 
	local_language_id INT NOT NULL, 
	[name] VARCHAR(79), 
	[description] TEXT, 
	PRIMARY KEY (item_flag_id, local_language_id), 
	FOREIGN KEY(item_flag_id) REFERENCES item_flags (id) ON UPDATE CASCADE ON DELETE NO ACTION, 
	FOREIGN KEY(local_language_id) REFERENCES languages (id) ON UPDATE CASCADE ON DELETE NO ACTION
);
INSERT INTO [item_flag_prose] (item_flag_id,local_language_id,[name],[description]) VALUES (1,9,'Countable','Has a count in the bag'),
 (2,9,'Consumable','Consumed when used'),
 (3,9,'Usable_overworld','Usable outside battle'),
 (4,9,'Usable_in_battle','Usable in battle'),
 (5,9,'Holdable','Can be held by a Pokémon'),
 (6,9,'Holdable_passive','Works passively when held'),
 (7,9,'Holdable_active','Usable by a Pokémon when held'),
 (8,9,'Underground','Appears in Sinnoh Underground'),
 (4,10,NULL,'Použitelný v souboji'),
 (5,10,'Držitelný','Může být držen pokémonem'),
 (3,10,NULL,'Použitelný mimo souboj'),
 (6,10,NULL,'Funguje pasivně při držení'),
 (8,10,'Podzemí','Nachází se v Sinnohském podzemí'),
 (2,10,'Spotřebující se','Spotřebuje se po použití'),
 (7,10,NULL,'Použitelný pokémonem při držení'),
 (1,10,'Počítatelný',NULL);
--GO
--DROP TABLE IF EXISTS item_flag_map;
CREATE TABLE item_flag_map (
	item_id INT NOT NULL, 
	item_flag_id INT NOT NULL, 
	PRIMARY KEY (item_id, item_flag_id), 
	FOREIGN KEY(item_id) REFERENCES items (id) ON UPDATE CASCADE ON DELETE NO ACTION, 
	FOREIGN KEY(item_flag_id) REFERENCES item_flags (id) ON UPDATE CASCADE ON DELETE NO ACTION
);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (1,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (1,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (1,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (1,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (2,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (2,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (2,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (2,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (3,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (3,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (3,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (3,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (4,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (4,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (4,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (4,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (5,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (5,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (5,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (5,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (6,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (6,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (6,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (6,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (7,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (7,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (7,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (7,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (8,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (8,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (8,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (8,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (9,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (9,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (9,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (9,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (10,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (10,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (10,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (10,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (11,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (11,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (11,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (11,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (12,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (12,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (12,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (12,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (13,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (13,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (13,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (13,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (14,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (14,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (14,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (14,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (15,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (15,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (15,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (15,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (16,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (16,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (16,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (16,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (17,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (17,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (17,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (17,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (17,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (18,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (18,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (18,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (18,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (18,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (19,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (19,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (19,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (19,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (19,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (20,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (20,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (20,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (20,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (20,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (21,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (21,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (21,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (21,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (21,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (22,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (22,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (22,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (22,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (22,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (23,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (23,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (23,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (23,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (23,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (24,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (24,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (24,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (24,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (24,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (25,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (25,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (25,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (25,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (25,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (26,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (26,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (26,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (26,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (26,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (27,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (27,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (27,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (27,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (27,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (28,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (28,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (28,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (28,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (28,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (28,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (29,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (29,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (29,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (29,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (29,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (29,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (30,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (30,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (30,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (30,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (30,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (31,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (31,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (31,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (31,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (31,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (32,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (32,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (32,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (32,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (32,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (33,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (33,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (33,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (33,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (33,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (34,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (34,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (34,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (34,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (34,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (35,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (35,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (35,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (35,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (35,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (36,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (36,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (36,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (36,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (36,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (37,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (37,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (37,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (37,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (37,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (38,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (38,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (38,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (38,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (38,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (39,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (39,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (39,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (39,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (39,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (40,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (40,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (40,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (40,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (40,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (41,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (41,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (41,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (41,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (41,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (42,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (42,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (42,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (42,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (42,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (43,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (43,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (43,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (43,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (43,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (44,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (44,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (44,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (44,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (44,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (45,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (45,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (45,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (45,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (45,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (46,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (46,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (46,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (46,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (46,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (47,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (47,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (47,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (47,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (47,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (48,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (48,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (48,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (48,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (48,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (49,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (49,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (49,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (49,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (49,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (50,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (50,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (50,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (50,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (50,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (51,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (51,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (51,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (51,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (51,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (52,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (52,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (52,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (52,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (52,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (53,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (53,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (53,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (53,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (53,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (54,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (54,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (54,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (54,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (54,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (55,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (55,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (55,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (55,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (56,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (56,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (56,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (56,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (57,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (57,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (57,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (57,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (58,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (58,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (58,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (58,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (59,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (59,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (59,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (59,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (60,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (60,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (60,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (60,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (61,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (61,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (61,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (61,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (62,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (62,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (62,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (62,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (63,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (63,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (63,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (63,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (64,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (64,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (64,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (64,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (65,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (65,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (65,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (65,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (65,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (66,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (66,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (66,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (66,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (67,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (67,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (67,4);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (67,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (68,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (68,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (68,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (68,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (69,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (69,2);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (69,3);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (69,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (70,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (71,1);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (72,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (73,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (74,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (75,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (80,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (81,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (82,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (83,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (84,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (85,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (91,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (93,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (99,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (100,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (101,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (102,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (103,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (104,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (105,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (106,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (110,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (111,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (112,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (113,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (126,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (127,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (128,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (129,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (130,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (131,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (132,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (133,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (134,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (135,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (136,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (137,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (138,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (139,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (140,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (161,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (162,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (163,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (164,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (165,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (166,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (167,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (168,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (169,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (170,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (171,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (172,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (173,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (174,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (175,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (176,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (177,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (178,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (179,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (180,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (181,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (182,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (183,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (184,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (185,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (186,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (187,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (188,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (189,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (190,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (190,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (191,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (191,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (192,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (192,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (193,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (193,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (194,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (194,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (195,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (195,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (196,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (196,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (197,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (197,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (198,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (198,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (199,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (199,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (200,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (200,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (201,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (202,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (203,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (204,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (205,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (205,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (206,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (206,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (206,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (207,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (207,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (208,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (208,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (209,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (209,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (210,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (210,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (211,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (211,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (213,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (214,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (214,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (215,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (215,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (215,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (216,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (216,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (217,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (217,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (218,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (218,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (219,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (219,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (220,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (220,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (221,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (221,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (222,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (222,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (223,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (223,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (224,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (224,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (225,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (225,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (226,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (226,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (227,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (227,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (228,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (228,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (230,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (230,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (231,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (231,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (232,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (232,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (233,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (234,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (235,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (236,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (237,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (238,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (239,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (240,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (241,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (242,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (242,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (243,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (243,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (244,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (244,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (245,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (245,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (246,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (246,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (246,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (247,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (247,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (248,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (248,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (249,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (249,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (250,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (250,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (251,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (252,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (252,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (253,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (253,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (254,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (254,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (255,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (255,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (255,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (256,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (256,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (257,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (257,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (258,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (258,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (259,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (259,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (259,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (260,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (260,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (260,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (261,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (261,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (261,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (262,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (262,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (262,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (263,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (263,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (264,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (264,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (265,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (265,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (266,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (266,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (267,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (267,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (268,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (268,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (269,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (269,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (270,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (270,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (271,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (271,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (272,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (272,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (273,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (273,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (274,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (274,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (275,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (275,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (275,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (276,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (276,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (276,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (277,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (277,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (277,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (278,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (278,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (278,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (279,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (279,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (279,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (280,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (280,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (280,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (281,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (281,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (281,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (282,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (282,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (282,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (283,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (283,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (283,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (284,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (284,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (284,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (285,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (285,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (285,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (286,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (286,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (286,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (287,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (287,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (287,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (288,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (288,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (288,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (289,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (289,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (289,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (290,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (290,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (290,8);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (291,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (291,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (292,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (292,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (293,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (293,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (294,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (294,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (295,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (295,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (296,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (296,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (297,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (303,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (303,7);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (304,5);
INSERT INTO [item_flag_map] (item_id,item_flag_id) VALUES (304,7);
--GO
--DROP TABLE IF EXISTS item_category_prose;
CREATE TABLE item_category_prose (
	item_category_id INT NOT NULL, 
	local_language_id INT NOT NULL, 
	[name] VARCHAR(79) NOT NULL, 
	PRIMARY KEY (item_category_id, local_language_id), 
	FOREIGN KEY(item_category_id) REFERENCES item_categories (id) ON UPDATE CASCADE ON DELETE NO ACTION, 
	FOREIGN KEY(local_language_id) REFERENCES languages (id) ON UPDATE CASCADE ON DELETE NO ACTION
);
INSERT INTO [item_category_prose] (item_category_id,local_language_id,[name]) VALUES (1,9,'Stat boosts'),
 (2,9,'Effort drop'),
 (3,9,'Medicine'),
 (4,9,'Other'),
 (5,9,'In a pinch'),
 (6,9,'Picky healing'),
 (7,9,'Type protection'),
 (8,9,'Baking only'),
 (9,9,'Collectibles'),
 (10,9,'Evolution'),
 (11,9,'Spelunking'),
 (12,9,'Held items'),
 (13,9,'Choice'),
 (14,9,'Effort training'),
 (15,9,'Bad held items'),
 (16,9,'Training'),
 (17,9,'Plates'),
 (18,9,'Species-specific'),
 (19,9,'Type enhancement'),
 (20,9,'Event items'),
 (21,9,'Gameplay'),
 (22,9,'Plot advancement'),
 (23,9,'Unused'),
 (24,9,'Loot'),
 (25,9,'All mail'),
 (26,9,'Vitamins'),
 (27,9,'Healing'),
 (28,9,'PP recovery'),
 (29,9,'Revival'),
 (30,9,'Status cures'),
 (32,9,'Mulch'),
 (33,9,'Special balls'),
 (34,9,'Standard balls'),
 (35,9,'Dex completion'),
 (36,9,'Scarves'),
 (37,9,'All machines'),
 (38,9,'Flutes'),
 (39,9,'Apricorn balls'),
 (40,9,'Apricorn Box'),
 (41,9,'Data Cards'),
 (42,9,'Jewels'),
 (43,9,'Miracle Shooter'),
 (44,9,'Mega Stones'),
 (10001,9,'X/Y unknown'),
 (4,10,'Zbytek'),
 (11,10,'Do jeskyní'),
 (5,10,'V nouzi'),
 (16,10,'Trénink'),
 (23,10,'Nepoužité'),
 (42,10,'Drahokamy'),
 (17,10,'Desky'),
 (28,10,'Léčící PP'),
 (29,10,'Oživující'),
 (8,10,'Jen k pečení'),
 (9,10,'Zběratelské'),
 (20,10,'Eventové'),
 (27,10,'Léčící'),
 (21,10,'Herní'),
 (32,10,'Hnojivo'),
 (7,10,'Typová protekce'),
 (26,10,'Vitamíny'),
 (22,10,'Příběhové'),
 (24,10,'Kořist'),
 (18,10,'Druhové');
--GO
--DROP TABLE IF EXISTS growth_rate_prose;
CREATE TABLE growth_rate_prose (
	growth_rate_id INT NOT NULL, 
	local_language_id INT NOT NULL, 
	[name] VARCHAR(79) NOT NULL, 
	PRIMARY KEY (growth_rate_id, local_language_id), 
	FOREIGN KEY(growth_rate_id) REFERENCES growth_rates (id) ON UPDATE CASCADE ON DELETE NO ACTION, 
	FOREIGN KEY(local_language_id) REFERENCES languages (id) ON UPDATE CASCADE ON DELETE NO ACTION
);
INSERT INTO [growth_rate_prose] (growth_rate_id,local_language_id,[name]) VALUES (1,5,'lente'),
 (1,6,'langsam'),
 (1,9,'slow'),
 (2,5,'moyenne'),
 (2,6,'mittel'),
 (2,9,'medium'),
 (3,5,'rapide'),
 (3,6,'schnell'),
 (3,9,'fast'),
 (4,5,'parabolique'),
 (4,6,'mittel langsam'),
 (4,9,'medium slow'),
 (5,5,'erratique'),
 (5,6,'langsam, dann sehr schnell'),
 (5,9,'slow then very fast'),
 (6,5,'fluctuante'),
 (6,6,'schnell, dann sehr langsam'),
 (6,9,'fast then very slow'),
 (1,10,'pomalá'),
 (4,10,'celkem pomalá'),
 (3,10,'rychlá'),
 (2,10,'průměrná');
INSERT INTO [generation_names] (generation_id,local_language_id,[name]) VALUES (1,5,'Génération I'),
 (1,6,'Generation I'),
 (1,9,'Generation I'),
 (2,5,'Génération II'),
 (2,6,'Generation II'),
 (2,9,'Generation II'),
 (3,5,'Génération III'),
 (3,6,'Generation III'),
 (3,9,'Generation III'),
 (4,5,'Génération IV'),
 (4,6,'Generation IV'),
 (4,9,'Generation IV'),
 (5,5,'Génération V'),
 (5,6,'Generation V'),
 (5,9,'Generation V'),
 (6,5,'Génération VI'),
 (6,6,'Generation VI'),
 (6,9,'Generation VI'),
 (1,10,'I. generace'),
 (4,10,'IV. generace'),
 (3,10,'III. generace'),
 (5,10,'V. generace'),
 (2,10,'II. generace');
--GO
--DROP TABLE IF EXISTS experience;
CREATE TABLE experience (
	growth_rate_id INT NOT NULL, 
	[level] INT NOT NULL, 
	experience INT NOT NULL, 
	PRIMARY KEY (growth_rate_id, level), 
	FOREIGN KEY(growth_rate_id) REFERENCES growth_rates (id) ON UPDATE CASCADE ON DELETE NO ACTION
);
INSERT INTO [experience] (growth_rate_id,level,experience) VALUES (1,1,0),
 (1,2,10),
 (1,3,33),
 (1,4,80),
 (1,5,156),
 (1,6,270),
 (1,7,428),
 (1,8,640),
 (1,9,911),
 (1,10,1250),
 (1,11,1663),
 (1,12,2160),
 (1,13,2746),
 (1,14,3430),
 (1,15,4218),
 (1,16,5120),
 (1,17,6141),
 (1,18,7290),
 (1,19,8573),
 (1,20,10000),
 (1,21,11576),
 (1,22,13310),
 (1,23,15208),
 (1,24,17280),
 (1,25,19531),
 (1,26,21970),
 (1,27,24603),
 (1,28,27440),
 (1,29,30486),
 (1,30,33750),
 (1,31,37238),
 (1,32,40960),
 (1,33,44921),
 (1,34,49130),
 (1,35,53593),
 (1,36,58320),
 (1,37,63316),
 (1,38,68590),
 (1,39,74148),
 (1,40,80000),
 (1,41,86151),
 (1,42,92610),
 (1,43,99383),
 (1,44,106480),
 (1,45,113906),
 (1,46,121670),
 (1,47,129778),
 (1,48,138240),
 (1,49,147061),
 (1,50,156250),
 (1,51,165813),
 (1,52,175760),
 (1,53,186096),
 (1,54,196830),
 (1,55,207968),
 (1,56,219520),
 (1,57,231491),
 (1,58,243890),
 (1,59,256723),
 (1,60,270000),
 (1,61,283726),
 (1,62,297910),
 (1,63,312558),
 (1,64,327680),
 (1,65,343281),
 (1,66,359370),
 (1,67,375953),
 (1,68,393040),
 (1,69,410636),
 (1,70,428750),
 (1,71,447388),
 (1,72,466560),
 (1,73,486271),
 (1,74,506530),
 (1,75,527343),
 (1,76,548720),
 (1,77,570666),
 (1,78,593190),
 (1,79,616298),
 (1,80,640000),
 (1,81,664301),
 (1,82,689210),
 (1,83,714733),
 (1,84,740880),
 (1,85,767656),
 (1,86,795070),
 (1,87,823128),
 (1,88,851840),
 (1,89,881211),
 (1,90,911250),
 (1,91,941963),
 (1,92,973360),
 (1,93,1005446),
 (1,94,1038230),
 (1,95,1071718),
 (1,96,1105920),
 (1,97,1140841),
 (1,98,1176490),
 (1,99,1212873),
 (1,100,1250000),
 (2,1,0),
 (2,2,8),
 (2,3,27),
 (2,4,64),
 (2,5,125),
 (2,6,216),
 (2,7,343),
 (2,8,512),
 (2,9,729),
 (2,10,1000),
 (2,11,1331),
 (2,12,1728),
 (2,13,2197),
 (2,14,2744),
 (2,15,3375),
 (2,16,4096),
 (2,17,4913),
 (2,18,5832),
 (2,19,6859),
 (2,20,8000),
 (2,21,9261),
 (2,22,10648),
 (2,23,12167),
 (2,24,13824),
 (2,25,15625),
 (2,26,17576),
 (2,27,19683),
 (2,28,21952),
 (2,29,24389),
 (2,30,27000),
 (2,31,29791),
 (2,32,32768),
 (2,33,35937),
 (2,34,39304),
 (2,35,42875),
 (2,36,46656),
 (2,37,50653),
 (2,38,54872),
 (2,39,59319),
 (2,40,64000),
 (2,41,68921),
 (2,42,74088),
 (2,43,79507),
 (2,44,85184),
 (2,45,91125),
 (2,46,97336),
 (2,47,103823),
 (2,48,110592),
 (2,49,117649),
 (2,50,125000),
 (2,51,132651),
 (2,52,140608),
 (2,53,148877),
 (2,54,157464),
 (2,55,166375),
 (2,56,175616),
 (2,57,185193),
 (2,58,195112),
 (2,59,205379),
 (2,60,216000),
 (2,61,226981),
 (2,62,238328),
 (2,63,250047),
 (2,64,262144),
 (2,65,274625),
 (2,66,287496),
 (2,67,300763),
 (2,68,314432),
 (2,69,328509),
 (2,70,343000),
 (2,71,357911),
 (2,72,373248),
 (2,73,389017),
 (2,74,405224),
 (2,75,421875),
 (2,76,438976),
 (2,77,456533),
 (2,78,474552),
 (2,79,493039),
 (2,80,512000),
 (2,81,531441),
 (2,82,551368),
 (2,83,571787),
 (2,84,592704),
 (2,85,614125),
 (2,86,636056),
 (2,87,658503),
 (2,88,681472),
 (2,89,704969),
 (2,90,729000),
 (2,91,753571),
 (2,92,778688),
 (2,93,804357),
 (2,94,830584),
 (2,95,857375),
 (2,96,884736),
 (2,97,912673),
 (2,98,941192),
 (2,99,970299),
 (2,100,1000000),
 (3,1,0),
 (3,2,6),
 (3,3,21),
 (3,4,51),
 (3,5,100),
 (3,6,172),
 (3,7,274),
 (3,8,409),
 (3,9,583),
 (3,10,800),
 (3,11,1064),
 (3,12,1382),
 (3,13,1757),
 (3,14,2195),
 (3,15,2700),
 (3,16,3276),
 (3,17,3930),
 (3,18,4665),
 (3,19,5487),
 (3,20,6400),
 (3,21,7408),
 (3,22,8518),
 (3,23,9733),
 (3,24,11059),
 (3,25,12500),
 (3,26,14060),
 (3,27,15746),
 (3,28,17561),
 (3,29,19511),
 (3,30,21600),
 (3,31,23832),
 (3,32,26214),
 (3,33,28749),
 (3,34,31443),
 (3,35,34300),
 (3,36,37324),
 (3,37,40522),
 (3,38,43897),
 (3,39,47455),
 (3,40,51200),
 (3,41,55136),
 (3,42,59270),
 (3,43,63605),
 (3,44,68147),
 (3,45,72900),
 (3,46,77868),
 (3,47,83058),
 (3,48,88473),
 (3,49,94119),
 (3,50,100000),
 (3,51,106120),
 (3,52,112486),
 (3,53,119101),
 (3,54,125971),
 (3,55,133100),
 (3,56,140492),
 (3,57,148154),
 (3,58,156089),
 (3,59,164303),
 (3,60,172800),
 (3,61,181584),
 (3,62,190662),
 (3,63,200037),
 (3,64,209715),
 (3,65,219700),
 (3,66,229996),
 (3,67,240610),
 (3,68,251545),
 (3,69,262807),
 (3,70,274400),
 (3,71,286328),
 (3,72,298598),
 (3,73,311213),
 (3,74,324179),
 (3,75,337500),
 (3,76,351180),
 (3,77,365226),
 (3,78,379641),
 (3,79,394431),
 (3,80,409600),
 (3,81,425152),
 (3,82,441094),
 (3,83,457429),
 (3,84,474163),
 (3,85,491300),
 (3,86,508844),
 (3,87,526802),
 (3,88,545177),
 (3,89,563975),
 (3,90,583200),
 (3,91,602856),
 (3,92,622950),
 (3,93,643485),
 (3,94,664467),
 (3,95,685900),
 (3,96,707788),
 (3,97,730138),
 (3,98,752953),
 (3,99,776239),
 (3,100,800000),
 (4,1,0),
 (4,2,9),
 (4,3,57),
 (4,4,96),
 (4,5,135),
 (4,6,179),
 (4,7,236),
 (4,8,314),
 (4,9,419),
 (4,10,560),
 (4,11,742),
 (4,12,973),
 (4,13,1261),
 (4,14,1612),
 (4,15,2035),
 (4,16,2535),
 (4,17,3120),
 (4,18,3798),
 (4,19,4575),
 (4,20,5460),
 (4,21,6458),
 (4,22,7577),
 (4,23,8825),
 (4,24,10208),
 (4,25,11735),
 (4,26,13411),
 (4,27,15244),
 (4,28,17242),
 (4,29,19411),
 (4,30,21760),
 (4,31,24294),
 (4,32,27021),
 (4,33,29949),
 (4,34,33084),
 (4,35,36435),
 (4,36,40007),
 (4,37,43808),
 (4,38,47846),
 (4,39,52127),
 (4,40,56660),
 (4,41,61450),
 (4,42,66505),
 (4,43,71833),
 (4,44,77440),
 (4,45,83335),
 (4,46,89523),
 (4,47,96012),
 (4,48,102810),
 (4,49,109923),
 (4,50,117360),
 (4,51,125126),
 (4,52,133229),
 (4,53,141677),
 (4,54,150476),
 (4,55,159635),
 (4,56,169159),
 (4,57,179056),
 (4,58,189334),
 (4,59,199999),
 (4,60,211060),
 (4,61,222522),
 (4,62,234393),
 (4,63,246681),
 (4,64,259392),
 (4,65,272535),
 (4,66,286115),
 (4,67,300140),
 (4,68,314618),
 (4,69,329555),
 (4,70,344960),
 (4,71,360838),
 (4,72,377197),
 (4,73,394045),
 (4,74,411388),
 (4,75,429235),
 (4,76,447591),
 (4,77,466464),
 (4,78,485862),
 (4,79,505791),
 (4,80,526260),
 (4,81,547274),
 (4,82,568841),
 (4,83,590969),
 (4,84,613664),
 (4,85,636935),
 (4,86,660787),
 (4,87,685228),
 (4,88,710266),
 (4,89,735907),
 (4,90,762160),
 (4,91,789030),
 (4,92,816525),
 (4,93,844653),
 (4,94,873420),
 (4,95,902835),
 (4,96,932903),
 (4,97,963632),
 (4,98,995030),
 (4,99,1027103),
 (4,100,1059860),
 (5,1,0),
 (5,2,15),
 (5,3,52),
 (5,4,122),
 (5,5,237),
 (5,6,406),
 (5,7,637),
 (5,8,942),
 (5,9,1326),
 (5,10,1800),
 (5,11,2369),
 (5,12,3041),
 (5,13,3822),
 (5,14,4719),
 (5,15,5737),
 (5,16,6881),
 (5,17,8155),
 (5,18,9564),
 (5,19,11111),
 (5,20,12800),
 (5,21,14632),
 (5,22,16610),
 (5,23,18737),
 (5,24,21012),
 (5,25,23437),
 (5,26,26012),
 (5,27,28737),
 (5,28,31610),
 (5,29,34632),
 (5,30,37800),
 (5,31,41111),
 (5,32,44564),
 (5,33,48155),
 (5,34,51881),
 (5,35,55737),
 (5,36,59719),
 (5,37,63822),
 (5,38,68041),
 (5,39,72369),
 (5,40,76800),
 (5,41,81326),
 (5,42,85942),
 (5,43,90637),
 (5,44,95406),
 (5,45,100237),
 (5,46,105122),
 (5,47,110052),
 (5,48,115015),
 (5,49,120001),
 (5,50,125000),
 (5,51,131324),
 (5,52,137795),
 (5,53,144410),
 (5,54,151165),
 (5,55,158056),
 (5,56,165079),
 (5,57,172229),
 (5,58,179503),
 (5,59,186894),
 (5,60,194400),
 (5,61,202013),
 (5,62,209728),
 (5,63,217540),
 (5,64,225443),
 (5,65,233431),
 (5,66,241496),
 (5,67,249633),
 (5,68,257834),
 (5,69,267406),
 (5,70,276458),
 (5,71,286328),
 (5,72,296358),
 (5,73,305767),
 (5,74,316074),
 (5,75,326531),
 (5,76,336255),
 (5,77,346965),
 (5,78,357812),
 (5,79,367807),
 (5,80,378880),
 (5,81,390077),
 (5,82,400293),
 (5,83,411686),
 (5,84,423190),
 (5,85,433572),
 (5,86,445239),
 (5,87,457001),
 (5,88,467489),
 (5,89,479378),
 (5,90,491346),
 (5,91,501878),
 (5,92,513934),
 (5,93,526049),
 (5,94,536557),
 (5,95,548720),
 (5,96,560922),
 (5,97,571333),
 (5,98,583539),
 (5,99,591882),
 (5,100,600000),
 (6,1,0),
 (6,2,4),
 (6,3,13),
 (6,4,32),
 (6,5,65),
 (6,6,112),
 (6,7,178),
 (6,8,276),
 (6,9,393),
 (6,10,540),
 (6,11,745),
 (6,12,967),
 (6,13,1230),
 (6,14,1591),
 (6,15,1957),
 (6,16,2457),
 (6,17,3046),
 (6,18,3732),
 (6,19,4526),
 (6,20,5440),
 (6,21,6482),
 (6,22,7666),
 (6,23,9003),
 (6,24,10506),
 (6,25,12187),
 (6,26,14060),
 (6,27,16140),
 (6,28,18439),
 (6,29,20974),
 (6,30,23760),
 (6,31,26811),
 (6,32,30146),
 (6,33,33780),
 (6,34,37731),
 (6,35,42017),
 (6,36,46656),
 (6,37,50653),
 (6,38,55969),
 (6,39,60505),
 (6,40,66560),
 (6,41,71677),
 (6,42,78533),
 (6,43,84277),
 (6,44,91998),
 (6,45,98415),
 (6,46,107069),
 (6,47,114205),
 (6,48,123863),
 (6,49,131766),
 (6,50,142500),
 (6,51,151222),
 (6,52,163105),
 (6,53,172697),
 (6,54,185807),
 (6,55,196322),
 (6,56,210739),
 (6,57,222231),
 (6,58,238036),
 (6,59,250562),
 (6,60,267840),
 (6,61,281456),
 (6,62,300293),
 (6,63,315059),
 (6,64,335544),
 (6,65,351520),
 (6,66,373744),
 (6,67,390991),
 (6,68,415050),
 (6,69,433631),
 (6,70,459620),
 (6,71,479600),
 (6,72,507617),
 (6,73,529063),
 (6,74,559209),
 (6,75,582187),
 (6,76,614566),
 (6,77,639146),
 (6,78,673863),
 (6,79,700115),
 (6,80,737280),
 (6,81,765275),
 (6,82,804997),
 (6,83,834809),
 (6,84,877201),
 (6,85,908905),
 (6,86,954084),
 (6,87,987754),
 (6,88,1035837),
 (6,89,1071552),
 (6,90,1122660),
 (6,91,1160499),
 (6,92,1214753),
 (6,93,1254796),
 (6,94,1312322),
 (6,95,1354652),
 (6,96,1415577),
 (6,97,1460276),
 (6,98,1524731),
 (6,99,1571884),
 (6,100,1640000);
--GO
--DROP TABLE IF EXISTS evolution_trigger_prose;
CREATE TABLE evolution_trigger_prose (
	evolution_trigger_id INT NOT NULL, 
	local_language_id INT NOT NULL, 
	[name] VARCHAR(79) NOT NULL, 
	PRIMARY KEY (evolution_trigger_id, local_language_id), 
	FOREIGN KEY(evolution_trigger_id) REFERENCES evolution_triggers (id) ON UPDATE CASCADE ON DELETE NO ACTION, 
	FOREIGN KEY(local_language_id) REFERENCES languages (id) ON UPDATE CASCADE ON DELETE NO ACTION
);
INSERT INTO [evolution_trigger_prose] (evolution_trigger_id,local_language_id,[name]) VALUES (1,5,'Montée de niveau'),
 (1,6,'Levelaufstieg'),
 (1,9,'Level up'),
 (2,5,'Échange'),
 (2,6,'Tausch'),
 (2,9,'Trade'),
 (3,5,'Utilisation d''un objet'),
 (3,6,'Gegenstand nutzen'),
 (3,9,'Use item'),
 (4,5,'Place dans l''équipe et une Poké Ball'),
 (4,6,'Platz im Team und ein Pokéball'),
 (4,9,'Shed');