0	0000	newobj	instance void RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::.ctor()
1	0005	stloc.0
2	0006	ldloc.0
3	0007	ldarg.0
4	0008	stfld	class RimWorld.WildAnimalSpawner RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::'<>4__this'
5	000D	ldloc.0
6	000E	ldarg.1
7	000F	stfld	valuetype Verse.IntVec3 RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::loc
8	0014	ldarg.3
9	0015	brtrue.s	28 (0055) ldarg.2 
10	0017	ldarg.0
11	0018	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
12	001D	ldloc.0
13	001E	ldfld	valuetype Verse.IntVec3 RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::loc
14	0023	call	class RimWorld.BiomeDef RimWorld.MapGenUtility::BiomeAt(class Verse.Map, valuetype Verse.IntVec3)
15	0028	callvirt	instance class [mscorlib]System.Collections.Generic.IEnumerable`1<class Verse.PawnKindDef> RimWorld.BiomeDef::get_AllWildAnimals()
16	002D	ldloc.0
17	002E	ldftn	instance bool RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::'<SpawnRandomWildAnimalAt>b__0'(class Verse.PawnKindDef)
18	0034	newobj	instance void class [mscorlib]System.Func`2<class Verse.PawnKindDef, bool>::.ctor(object, native int)
19	0039	call	class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0> [System.Core]System.Linq.Enumerable::Where<class Verse.PawnKindDef>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, bool>)
20	003E	ldloc.0
21	003F	ldftn	instance float32 RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::'<SpawnRandomWildAnimalAt>b__1'(class Verse.PawnKindDef)
22	0045	newobj	instance void class [mscorlib]System.Func`2<class Verse.PawnKindDef, float32>::.ctor(object, native int)
23	004A	ldarga.s	animalKind (3)
24	004C	call	bool Verse.GenCollection::TryRandomElementByWeight<class Verse.PawnKindDef>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>, class [mscorlib]System.Func`2<!!0, float32>, !!0&)
25	0051	brtrue.s	28 (0055) ldarg.2 
26	0053	ldc.i4.0
27	0054	ret
28	0055	ldarg.2
29	0056	brfalse.s	41 (0077) ldc.i4.0 
30	0058	ldarg.3
31	0059	callvirt	instance class Verse.RaceProperties Verse.PawnKindDef::get_RaceProps()
32	005E	ldfld	bool Verse.RaceProperties::canFlyIntoMap
33	0063	brfalse.s	41 (0077) ldc.i4.0 
34	0065	ldarg.0
35	0066	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
36	006B	ldsfld	class Verse.Map Verse.MapGenerator::mapBeingGenerated
37	0070	ceq
38	0072	ldc.i4.0
39	0073	ceq
40	0075	br.s	42 (0078) stloc.1 
41	0077	ldc.i4.0
42	0078	stloc.1
43	0079	ldloc.1
44	007A	brfalse.s	79 (00E7) ldc.i4.0 
45	007C	newobj	instance void RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_1'::.ctor()
46	0081	stloc.s	V_4 (4)
47	0083	ldloc.s	V_4 (4)
48	0085	ldloc.0
49	0086	stfld	class RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0' RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_1'::'CS$<>8__locals1'
50	008B	ldloc.s	V_4 (4)
51	008D	ldarg.3
52	008E	callvirt	instance class Verse.RaceProperties Verse.PawnKindDef::get_RaceProps()
53	0093	ldfld	bool Verse.RaceProperties::waterSeeker
54	0098	brfalse.s	60 (00AC) ldc.i4.0 
55	009A	ldarg.0
56	009B	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
57	00A0	ldfld	class Verse.TerrainGrid Verse.Map::terrainGrid
58	00A5	callvirt	instance bool Verse.TerrainGrid::get_AnyWaterCells()
59	00AA	br.s	61 (00AD) stfld bool RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_1'::anyWaterForSeekers
60	00AC	ldc.i4.0
61	00AD	stfld	bool RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_1'::anyWaterForSeekers
62	00B2	ldloc.s	V_4 (4)
63	00B4	ldftn	instance bool RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_1'::'<SpawnRandomWildAnimalAt>b__2'(valuetype Verse.IntVec3)
64	00BA	newobj	instance void class [mscorlib]System.Predicate`1<valuetype Verse.IntVec3>::.ctor(object, native int)
65	00BF	ldarg.0
66	00C0	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
67	00C5	ldc.i4	1000
68	00CA	ldloca.s	V_5 (5)
69	00CC	call	bool Verse.CellFinderLoose::TryGetRandomCellWith(class [mscorlib]System.Predicate`1<valuetype Verse.IntVec3>, class Verse.Map, int32, valuetype Verse.IntVec3&)
70	00D1	brfalse.s	76 (00E3) ldc.i4.0 
71	00D3	ldloc.s	V_4 (4)
72	00D5	ldfld	class RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0' RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_1'::'CS$<>8__locals1'
73	00DA	ldloc.s	V_5 (5)
74	00DC	stfld	valuetype Verse.IntVec3 RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::loc
75	00E1	br.s	81 (00E9) ldarg.0 
76	00E3	ldc.i4.0
77	00E4	stloc.1
78	00E5	br.s	81 (00E9) ldarg.0 
79	00E7	ldc.i4.0
80	00E8	stloc.1
81	00E9	ldarg.0
82	00EA	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
83	00EF	callvirt	instance class RimWorld.Planet.Tile Verse.Map::get_TileInfo()
84	00F4	callvirt	instance bool RimWorld.Planet.Tile::get_IsCoastal()
85	00F9	brfalse.s	118 (0165) ldarg.3 
86	00FB	ldarg.0
87	00FC	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
88	0101	ldloc.0
89	0102	ldfld	valuetype Verse.IntVec3 RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::loc
90	0107	call	class RimWorld.BiomeDef RimWorld.MapGenUtility::BiomeAt(class Verse.Map, valuetype Verse.IntVec3)
91	010C	ldarg.3
92	010D	callvirt	instance bool RimWorld.BiomeDef::ShouldSpawnAnimalOnCoast(class Verse.PawnKindDef)
93	0112	brfalse.s	118 (0165) ldarg.3 
94	0114	newobj	instance void RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_2'::.ctor()
95	0119	stloc.s	V_6 (6)
96	011B	ldloc.s	V_6 (6)
97	011D	ldloc.0
98	011E	stfld	class RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0' RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_2'::'CS$<>8__locals2'
99	0123	ldloc.s	V_6 (6)
100	0125	ldarg.0
101	0126	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
102	012B	ldfld	class Verse.TerrainGrid Verse.Map::terrainGrid
103	0130	stfld	class Verse.TerrainGrid RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_2'::terrainGrid
104	0135	ldloca.s	V_7 (7)
105	0137	ldarg.0
106	0138	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
107	013D	ldsfld	float32 Verse.CellFinder::EdgeRoadChance_Ignore
108	0142	ldc.i4.1
109	0143	ldloc.s	V_6 (6)
110	0145	ldftn	instance bool RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_2'::'<SpawnRandomWildAnimalAt>b__3'(valuetype Verse.IntVec3)
111	014B	newobj	instance void class [mscorlib]System.Predicate`1<valuetype Verse.IntVec3>::.ctor(object, native int)
112	0150	call	bool RimWorld.RCellFinder::TryFindRandomPawnEntryCell(valuetype Verse.IntVec3&, class Verse.Map, float32, bool, class [mscorlib]System.Predicate`1<valuetype Verse.IntVec3>)
113	0155	brfalse.s	118 (0165) ldarg.3 
114	0157	ldloc.s	V_6 (6)
115	0159	ldfld	class RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0' RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_2'::'CS$<>8__locals2'
116	015E	ldloc.s	V_7 (7)
117	0160	stfld	valuetype Verse.IntVec3 RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::loc
118	0165	ldarg.3
119	0166	ldflda	valuetype Verse.IntRange Verse.PawnKindDef::wildGroupSize
120	016B	call	instance int32 Verse.IntRange::get_RandomInRange()
121	0170	stloc.2
122	0171	ldarg.3
123	0172	ldflda	valuetype Verse.IntRange Verse.PawnKindDef::wildGroupSize
124	0177	ldfld	int32 Verse.IntRange::max
125	017C	conv.r4
126	017D	call	float32 [UnityEngine.CoreModule]UnityEngine.Mathf::Sqrt(float32)
127	0182	call	int32 [UnityEngine.CoreModule]UnityEngine.Mathf::CeilToInt(float32)
128	0187	stloc.3
129	0188	ldc.i4.0
130	0189	stloc.s	V_8 (8)
131	018B	br	197 (0243) ldloc.s V_8 (8)
132	0190	ldloc.0
133	0191	ldfld	valuetype Verse.IntVec3 RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::loc
134	0196	ldarg.0
135	0197	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
136	019C	ldloc.3
137	019D	ldnull
138	019E	call	valuetype Verse.IntVec3 Verse.CellFinder::RandomClosewalkCellNear(valuetype Verse.IntVec3, class Verse.Map, int32, class [mscorlib]System.Predicate`1<valuetype Verse.IntVec3>)
139	01A3	stloc.s	V_9 (9)
140	01A5	ldarg.3
141	01A6	ldnull
142	01A7	ldloca.s	V_11 (11)
143	01A9	initobj	valuetype [mscorlib]System.Nullable`1<valuetype RimWorld.Planet.PlanetTile>
144	01AF	ldloc.s	V_11 (11)
145	01B1	call	class Verse.Pawn Verse.PawnGenerator::GeneratePawn(class Verse.PawnKindDef, class RimWorld.Faction, valuetype [mscorlib]System.Nullable`1<valuetype RimWorld.Planet.PlanetTile>)
146	01B6	stloc.s	V_10 (10)
147	01B8	ldarg.0
148	01B9	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
149	01BE	ldloc.0
150	01BF	ldfld	valuetype Verse.IntVec3 RimWorld.WildAnimalSpawner/'<>c__DisplayClass15_0'::loc
151	01C4	call	class RimWorld.BiomeDef RimWorld.MapGenUtility::BiomeAt(class Verse.Map, valuetype Verse.IntVec3)
152	01C9	ldfld	float32 RimWorld.BiomeDef::wildAnimalScariaChance
153	01CE	call	bool Verse.Rand::Chance(float32)
154	01D3	brfalse.s	165 (01F3) ldloc.1 
155	01D5	ldloc.s	V_10 (10)
156	01D7	ldfld	class Verse.Pawn_HealthTracker Verse.Pawn::health
157	01DC	ldsfld	class Verse.HediffDef RimWorld.HediffDefOf::Scaria
158	01E1	ldnull
159	01E2	ldloca.s	V_12 (12)
160	01E4	initobj	valuetype [mscorlib]System.Nullable`1<valuetype Verse.DamageInfo>
161	01EA	ldloc.s	V_12 (12)
162	01EC	ldnull
163	01ED	callvirt	instance class Verse.Hediff Verse.Pawn_HealthTracker::AddHediff(class Verse.HediffDef, class Verse.BodyPartRecord, valuetype [mscorlib]System.Nullable`1<valuetype Verse.DamageInfo>, class Verse.DamageWorker/DamageResult)
164	01F2	pop
165	01F3	ldloc.1
166	01F4	brfalse.s	186 (022C) ldloc.s V_10 (10)
167	01F6	ldsfld	class Verse.ThingDef RimWorld.ThingDefOf::FlyerArrival
168	01FB	ldloc.s	V_10 (10)
169	01FD	call	class RimWorld.Skyfaller RimWorld.SkyfallerMaker::MakeSkyfaller(class Verse.ThingDef, class Verse.Thing)
170	0202	ldloc.s	V_9 (9)
171	0204	ldarg.0
172	0205	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
173	020A	ldc.i4.1
174	020B	ldnull
175	020C	ldnull
176	020D	ldloca.s	V_13 (13)
177	020F	initobj	valuetype [mscorlib]System.Nullable`1<valuetype Verse.Rot4>
178	0215	ldloc.s	V_13 (13)
179	0217	ldc.i4.1
180	0218	call	bool Verse.GenPlace::TryPlaceThing(class Verse.Thing, valuetype Verse.IntVec3, class Verse.Map, valuetype Verse.ThingPlaceMode, class [mscorlib]System.Action`2<class Verse.Thing, int32>, class [mscorlib]System.Predicate`1<valuetype Verse.IntVec3>, valuetype [mscorlib]System.Nullable`1<valuetype Verse.Rot4>, int32)
181	021D	pop
182	021E	ldloc.s	V_10 (10)
183	0220	ldsfld	valuetype Verse.Rot4 Verse.Rot4::East
184	0225	callvirt	instance void Verse.Thing::set_Rotation(valuetype Verse.Rot4)
185	022A	br.s	193 (023D) ldloc.s V_8 (8)
186	022C	ldloc.s	V_10 (10)
187	022E	ldloc.s	V_9 (9)
188	0230	ldarg.0
189	0231	ldfld	class Verse.Map RimWorld.WildAnimalSpawner::map
190	0236	ldc.i4.0
191	0237	call	class Verse.Thing Verse.GenSpawn::Spawn(class Verse.Thing, valuetype Verse.IntVec3, class Verse.Map, valuetype Verse.WipeMode)
192	023C	pop
193	023D	ldloc.s	V_8 (8)
194	023F	ldc.i4.1
195	0240	add
196	0241	stloc.s	V_8 (8)
197	0243	ldloc.s	V_8 (8)
198	0245	ldloc.2
199	0246	blt	132 (0190) ldloc.0 
200	024B	ldc.i4.1
201	024C	ret
