Info
--

Simple EHR database solution benchmark research project.
It is used to measure different EHR document storing databases (for example, this project measures performance for CouchDB).


Queries
--

Without childs:
```javascript
	function(doc) {
		if (doc.archetype_node_id)
  			emit(doc.archetype_node_id, doc);
	}
```

With child select:
```javascript
	function(doc) {
  		emit([doc.archetype_node_id, doc.type_name], doc);
	  	for (var i = 0 in doc.items) {
  	 		emit([doc.items[i].archetype_node_id, doc.items[i].type_name], doc);
	  	}
	}
```

License
--

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

