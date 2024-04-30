# MongoDBConsoleApp
Written Solutions for StackOverflow questions.

**Reason to contribute:**
- Providing (reading) shortcut for referring questions and its solution source code.

**Structure:**

- Program.cs: Entry point for calling the solution methods
- ISolution.cs: An interface for `Solution` class.
- Solution_XXX.cs: A class file which implements `ISolution` and with the written answer.
- Data/Solution_XXX_`<Collection?>`.json: Sample data collection for `Solution` class.

**Solutions and Linked question:**

| File | Question |
|-|-|
| [Solution_001][1] | [MongoDB .Net Driver - Filter Builder throwing an exception][2] |
| [Solution_002][3] | [MongoDB .Net Driver - Pull multiple elements from arrays that exist in multiple documents][4] |
| [Solution_003][5] | [Query with filter builder on nested array using MongoDB C# driver with a given array of string][6] |
| [Solution_004][7] | [MongoDB .NET Driver - StartsWith & Contains with loosely typed data][8] |
| [Solution_005][9] | [MongoDB Driver Builders&lt;dynamic&gt; dont work on equal to date][10] |
| [Solution_006][11] | [MongoDB .NET Driver - Pagination on array stored in a document field][12] |
| [Solution_007][13] | [How to Filter and get last entry based on date using C# and Mongo][14] |
| [Solution_008][15] | [Retrieving list of documents from collection by id in nested list][16] |
| [Solution_009][17] | [No array filter found for identifier '-1'][18] |
| [Solution_010][19] | [Filter.Lte(x=>x.Price, "9") getting wrong results][20] |
| [Solution_011][21] | [Rename nested field in BsonDocument with MongoDB.Driver][22] |
| [Solution_012][23] | [MongoDB .Net Driver update Cannot use the part ... to traverse the element][24] |
| [Solution_013][25] |  |
| [Solution_014][27] | [MongoDB.NET Driver query for $lte and $gte throwing error: An object representing an expression must have exactly one field][28] |
| [Solution_015][29] | [Convert lambda expressions to json objects using MongoDB.Driver][30] |
| [Solution_016][31] | [How to write this MongoDB (Aggregate) query into C#][32] |
| [Solution_017][33] | [Issue with data return in MongoDB][34] |
| [Solution_018][35] | [C# MongoDB - How to select nested properties only][36] |
| [Solution_019][37] | <ul><li>[Not equal to operator for MongoDB query in C#][38]</li> <li>[C# MongoDb Driver Convert string to DateTime and for Filter Builder][39]</li></ul> |
| [Solution_020][40] | [MongoDB - How to push an embedded array into an existing document][41] |
| [Solution_021][42] | [c# and mongodb include or exclude element of array using projection][43] |
| [Solution_022][44] | [MongoDB C# Driver: Nested Lookups - How do I "join" nested relations?][45] |
| [Solution_024][48] | [How can I flatten this array of subdocuments?][49] |
| [Solution_025][50] | [How to "aggregate" with "project.priority" in MongoDB query in C#?][51] |
| [Solution_026][52] | [How to perform like on MongoDB document for integer values][53] |
| [Solution_027][54] | <ul><li>[How to create an expression in C# from string?][55]</li> <li>[How can I materialize a string to the actual type?][56]</li></ul> |
| [Solution_028][57] | [MongoDB - Count unread messages][58] |
| [Solution_029][59] | [Find lastly added collection with some condition in MongoDB and C#][60] |
| [Solution_030][61] | [C# Searching MongoDB string that starts with "xyz"][62] |
| [Solution_031][63] | [MongoDB LINQ - How get all keys in all level in a collection][64] |
| [Solution_032][65] | [MongoDB .NET Driver - Increment a value inside dictionary][66] |
| [Solution_033][67] | [MongoDB - Search a field of type BsonDocument by their values][68] |
| [Solution_034][69] | [MongoDb return filtered array elements out of one document in C#][70] |
| [Solution_036][71] | [MongoDB .NET Driver - Aggregate group and count][72] |
| [Solution_037][73] | [How to IndexKeysDefinitionBuilder change to IndexKeysDefinition (MongoDB in C#)][74] |
| [Solution_038][75] | [MongoDB - How to update a single object in the array of objects inside a document][76] |
| [Solution_039][77] | [MongoDB - Query max date in collection][78] |
| [Solution_041][79] | [MongoDB .NET Driver - Count and average on lookup field][80] |
| [Solution_042][81] | [MongoDB - Cannot create field 'ChildProperty' in element with ParentProperty is null][82] |
| [Solution_043][83] | [MongoDB .NET Driver - How to push an element into an array which is inside an array][84] |
| [Solution_045][85] | [.Net Core MongoDB.Driver ObjectId Null from POCO Mapping][86] |
| [Solution_046][87] | [MongoDB .NET Driver - Update Item in Set][88] |
| [Solution_047][89] | [MongoDB .NET Driver - How to search the document(s) with fulfilling in the nested documents of an array][90] |
| [Solution_048][91] | [MongoDB - Get all elements inside a BsonArray and convert into List&lt;string&gt;][92] |
| [Solution_049][93] | [MongoDB .NET Driver - Using ElemMatch with FilterDefiniton][94] |
| [Solution_050][95] | [In MongoDB C# how to get nested array to perform aggregation query on it][96] |
| [Solution_051][97] | [MongoDB .NET Driver - Using pullFilter to remove string from string array][98] |
| [Solution_052][99] | [MongoDB .NET Driver - Aggregation query with calculation based on category][100] |
| [Solution_053][125] | [MongoDB .NET Driver - How to access nested element][126] |
| [Solution_055][101] | [newbie to CosmoDB how to query collection with multiple values?][102] |
| [Solution_056][103] | [Need help - Filter list of array based on specific item id using MongoDb C# driver][104] |
| [Solution_057][105] | [Cannot dynamically create an instance of type 'System.Text.Json.Nodes.JsonObject'. Reason: No parameterless constructor defined][106] |
| [Solution_058][107] | [MongoDB C#/.NET Driver - How to deserialize UUID and ISODate][108] |
| [Solution_059][109] | [MongoDB - How do I join the second collection to a child document using LINQ][110] |
| [Solution_060][111] | [Increment all fields of a MongoDB document from c#][112] |
| [Solution_061][113] | [MongoDB .NET Driver - How to increment double nested field][114] |
| [Solution_062][115] | [MongoDB - Select rows by their sequence number in one query][116] |
| [Solution_063][117] | [How to filter a collection for nested objects with different types][118] |
| [Solution_064][119] | [Using `$toLong` in MongoDB C# Queries][120] |
| [Solution_065][121] | [MongoDB .NET Driver - $lookup result to one merged & grouped array][122] |
| [Solution_068][123] | [How to filter using FHIR Identifiers from C# to mongoDB][124] |
| [Solution_069][152] | [MongoDB - How to query with multiple AND conditions for phone numbers using FilterDefinition][153] |
| [Solution_070][127] | [How to check last element's property of an array using MongoDB C# driver][128] |
| [Solution_071][129] | [Serialization inside Mongodb driver filter][130] |
| [Solution_072][131] | [MongoDB C# Property serializing string and int for query][132] |


[1]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_001.cs
[2]: https://stackoverflow.com/questions/69079627/mongodb-net-driver-filter-builder-throwing-an-exception/69414324#69414324

[3]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_002.cs
[4]: https://stackoverflow.com/questions/69403622/mongodb-net-driver-pull-multiple-elements-from-arrays-that-exist-in-multiple/69422853#69422853

[5]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_003.cs
[6]: https://stackoverflow.com/questions/69582406/query-with-filter-builder-on-nested-array-using-mongodb-c-sharp-driver-with-a-gi/69583877#69583877

[7]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_004.cs
[8]: https://stackoverflow.com/questions/69601591/mongodb-net-driver-startswith-contains-with-loosely-typed-data/69601745#69601745

[9]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_005.cs
[10]: https://stackoverflow.com/questions/69983653/mongodb-driver-buildersdynamic-dont-work-on-equal-to-date/69984438#69984438

[11]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_006.cs
[12]: https://stackoverflow.com/questions/70242147/mongodb-net-driver-pagination-on-array-stored-in-a-document-field/70242430#70242430

[13]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_007.cs
[14]: https://stackoverflow.com/questions/70298251/how-to-filter-and-get-last-entry-based-on-date-using-c-sharp-and-mongo/70299082#70299082

[15]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_008.cs
[16]: https://stackoverflow.com/questions/70660236/retrieving-list-of-documents-from-collection-by-id-in-nested-list/70660552#70660552

[17]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_009.cs
[18]: https://stackoverflow.com/questions/70702726/no-array-filter-found-for-identifier-1/70705559#70705559

[19]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_010.cs
[20]: https://stackoverflow.com/questions/70729292/filter-ltex-x-price-9-getting-wrong-results/70729526#70729526

[21]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_011.cs
[22]: https://stackoverflow.com/questions/70742881/rename-nested-field-in-bsondocument-with-mongodb-driver/70749460#70749460
  
[23]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_012.cs
[24]: https://stackoverflow.com/questions/70753460/mongodb-net-driver-update-cannot-use-the-part-to-traverse-the-element/70754899#70754899
  
[25]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_013.cs

  
[27]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_014.cs
[28]: https://stackoverflow.com/questions/70765835/mongodb-net-driver-query-for-lte-and-gte-throwing-error-an-object-representin/70767786#70767786
  
[29]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_015.cs
[30]: https://stackoverflow.com/questions/70795342/convert-lambda-expressions-to-json-objects-using-mongodb-driver
  
[31]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_016.cs
[32]: https://stackoverflow.com/questions/70839664/how-to-write-this-mongodb-aggregate-query-into-c-sharp/70842890#70842890
  
[33]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_017.cs
[34]: https://stackoverflow.com/questions/70928764/issue-with-data-return-in-mongodb/70934884#70934884
  
[35]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_018.cs
[36]: https://stackoverflow.com/questions/71069862/c-sharp-mongodb-how-to-select-nested-properties-only/71077234#71077234
  
[37]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_019.cs
[38]: https://stackoverflow.com/questions/71248390/not-equal-to-operator-for-mongodb-query-in-c-sharp/71248995#71248995
[39]: https://stackoverflow.com/questions/71421359/c-sharp-mongodb-driver-convert-string-to-datetime-and-for-filter-builder/71422269#71422269
  
[40]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_020.cs
[41]: https://stackoverflow.com/questions/71505601/mongodb-how-to-push-an-embedded-array-into-an-existing-document/71506079#71506079

[42]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_021.cs
[43]: https://stackoverflow.com/questions/71541098/c-sharp-and-mongodb-include-or-exclude-element-of-array-using-projection/71543925#71543925

[44]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_022.cs
[45]: https://stackoverflow.com/questions/71579890/mongodb-c-sharp-driver-nested-lookups-how-do-i-join-nested-relations/71582556#71582556

[48]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_024.cs
[49]: https://stackoverflow.com/questions/71693040/how-can-i-flatten-this-array-of-subdocuments/71693509#71693509

[50]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_025.cs
[51]: https://stackoverflow.com/questions/72170723/how-to-aggregate-with-project-priority-in-mongodb-query-in-c/72171247#72171247

[52]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_026.cs
[53]: https://stackoverflow.com/questions/72538606/how-to-perform-like-on-mongodb-document-for-integer-values/72540375#72540375
  
[54]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_027.cs
[55]: https://stackoverflow.com/questions/72809794/how-to-create-an-expression-in-c-sharp-from-string
[56]: https://stackoverflow.com/questions/72794815/how-can-i-materialize-a-string-to-the-actual-type/72795479#72795479
  
[57]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_028.cs
[58]: https://stackoverflow.com/questions/72968801/mongodb-count-unread-messages/72974256#72974256

[59]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_029.cs
[60]: https://stackoverflow.com/questions/73109463/find-lastly-added-collection-with-some-condition-in-mongodb-and-c-sharp/73109820#73109820

[61]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_030.cs
[62]: https://stackoverflow.com/questions/73267736/c-sharp-searching-mongodb-string-that-starts-with-xyz/73267990#73267990

[63]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_031.cs
[64]: https://stackoverflow.com/questions/73299041/mongodb-linq-how-get-all-keys-in-all-level-in-a-collection/73299815#73299815

[65]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_032.cs
[66]: https://stackoverflow.com/questions/73298130/mongodb-net-driver-increment-a-value-inside-dictionary/73300031#73300031

[67]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_033.cs
[68]: https://stackoverflow.com/questions/73352502/mongodb-search-a-field-of-type-bsondocument-by-their-values/73356181#73356181

[69]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_034.cs
[70]: https://stackoverflow.com/questions/73437201/mongodb-return-filtered-array-elements-out-of-one-document-in-c-sharp/73439241#73439241

[71]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_036.cs
[72]: https://stackoverflow.com/questions/73488700/mongodb-net-driver-aggregate-group-and-count/73499788#73499788

[73]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_037.cs
[74]: https://stackoverflow.com/questions/73488027/how-to-indexkeysdefinitionbuilder-change-to-indexkeysdefinition-mongodb-in-c/73508124#73508124

[75]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_038.cs
[76]: https://stackoverflow.com/questions/73517217/mongodb-how-to-update-a-single-object-in-the-array-of-objects-inside-a-documen

[77]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_039.cs
[78]: https://stackoverflow.com/questions/73570202/mongodb-query-max-date-in-collection/73577781#73577781

[79]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_041.cs
[80]: https://stackoverflow.com/questions/73591964/mongodb-net-driver-count-and-average-on-lookup-field/73592593#73592593

[81]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_042.cs
[82]: https://stackoverflow.com/questions/73628953/mongodb-cannot-create-field-childproperty-in-element-with-parentproperty-is/73629356#73629356

[83]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_043.cs
[84]: https://stackoverflow.com/questions/73704002/mongodb-net-driver-how-to-push-an-element-into-an-array-which-is-inside-an-ar/73710622#73710622

[85]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_045.cs
[86]: https://stackoverflow.com/questions/73738948/net-core-mongodb-driver-objectid-null-from-poco-mapping/73739244#73739244

[87]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_046.cs
[88]: https://stackoverflow.com/questions/74065603/mongodb-net-driver-update-item-in-set/74065904#74065904

[89]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_047.cs
[90]: https://stackoverflow.com/questions/74131498/mongodb-net-driver-how-to-search-the-documents-with-fulfilling-in-the-neste/74133584#74133584

[91]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_048.cs
[92]: https://stackoverflow.com/questions/74214246/mongodb-get-all-elements-inside-a-bsonarray-and-convert-into-liststring/74215348#74215348

[93]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_049.cs
[94]: https://stackoverflow.com/questions/74285939/mongodb-net-driver-using-elemmatch-with-filterdefiniton/74286414#74286414

[95]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_050.cs
[96]: https://stackoverflow.com/questions/74294539/in-mongodb-c-sharp-how-to-get-nested-array-to-perform-aggregation-query-on-it/74341177#74341177

[97]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_051.cs
[98]: https://stackoverflow.com/questions/74570841/mongodb-net-driver-using-pullfilter-to-remove-string-from-string-array/74587442#74587442

[99]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_052.cs
[100]: https://stackoverflow.com/questions/74509024/mongodb-net-driver-aggregation-query-with-calculation-based-on-category/74590498#74590498

[101]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_055.cs
[102]: https://stackoverflow.com/questions/74883970/newbie-to-cosmodb-how-to-query-collection-with-multiple-values/74884127#

[103]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_056.cs
[104]: https://stackoverflow.com/questions/74909307/need-help-filter-list-of-array-based-on-specific-item-id-using-mongodb-c-sharp/74917629#74917629

[105]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_057.cs
[106]: https://stackoverflow.com/questions/75000121/cannot-dynamically-create-an-instance-of-type-system-text-json-nodes-jsonobject/75000361#75000361

[107]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_058.cs
[108]: https://stackoverflow.com/questions/75067318/mongodb-c-net-driver-how-to-deserialize-uuid-and-isodate/75071541#75071541

[109]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_059.cs
[110]: https://stackoverflow.com/questions/75074400/mongodb-how-do-i-join-the-second-collection-to-a-child-document-using-linq/75078153#75078153

[111]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_060.cs
[112]: https://stackoverflow.com/questions/75159919/increment-all-fields-of-a-mongodb-document-from-c-sharp/75167534#75167534

[113]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_061.cs
[114]: https://stackoverflow.com/questions/75212463/mongodb-net-driver-how-to-increment-double-nested-field/75229701#75229701

[115]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_062.cs
[116]: https://stackoverflow.com/questions/75240005/mongodb-select-rows-by-their-sequence-number-in-one-query/75242146#75242146

[117]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_063.cs
[118]: https://stackoverflow.com/questions/75259584/how-to-filter-a-collection-for-nested-objects-with-different-types/75280593#75280593

[119]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_064.cs
[120]: https://stackoverflow.com/questions/75659073/using-tolong-in-mongodb-c-sharp-queries/75659600#75659600

[121]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_065.cs
[122]: https://stackoverflow.com/questions/75696037/mongodb-net-driver-lookup-result-to-one-merged-grouped-array/75697095#75697095

[123]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_068.cs
[124]: https://stackoverflow.com/questions/75859023/how-to-filter-using-fhir-identifiers-from-c-sharp-to-mongodb/75866267#75866267

[125]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_053.cs
[126]: https://stackoverflow.com/questions/73203113/mongodb-net-driver-how-to-access-nested-element/74600693#74600693

[127]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_070.cs
[128]: https://stackoverflow.com/questions/76768682/how-to-check-last-elements-property-of-an-array-using-mongodb-c-sharp-driver/76769126#76769126

[129]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_071.cs
[130]: https://stackoverflow.com/questions/76782517/serialization-inside-mongodb-driver-filter/76784612#76784612

[131]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_072.cs
[132]: https://stackoverflow.com/questions/76803275/mongodb-c-sharp-property-serializing-string-and-int-for-query/76804531#76804531

[152]: https://github.com/yongshun950824/MongoDBConsoleApp/blob/master/MongoDBConsoleApp/Solutions/Solution_069.cs
[153]: https://stackoverflow.com/questions/76740049/mongodb-how-to-query-with-multiple-and-conditions-for-phone-numbers-using-filt/76742141#76742141
