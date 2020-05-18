(function(){

this.MooTools = {
	version: '1.6.0',
	build: '529422872adfff401b901b8b6c7ca5114ee95e2b'
};

// typeOf, instanceOf

var typeOf = this.typeOf = function(item){
	if (item == null) return 'null';
	if (item.$family != null) return item.$family();

	if (item.nodeName){
		if (item.nodeType == 1) return 'element';
		if (item.nodeType == 3) return (/\S/).test(item.nodeValue) ? 'textnode' : 'whitespace';
	} else if (typeof item.length == 'number'){
		if ('callee' in item) return 'arguments';
		if ('item' in item) return 'collection';
	}

	return typeof item;
};

var instanceOf = this.instanceOf = function(item, object){
	if (item == null) return false;
	var constructor = item.$constructor || item.constructor;
	while (constructor){
		if (constructor === object) return true;
		constructor = constructor.parent;
	}
	/*<ltIE8>*/
	if (!item.hasOwnProperty) return false;
	/*</ltIE8>*/
	return item instanceof object;
};

var hasOwnProperty = Object.prototype.hasOwnProperty;

/*<ltIE8>*/
var enumerables = true;
for (var i in {toString: 1}) enumerables = null;
if (enumerables) enumerables = ['hasOwnProperty', 'valueOf', 'isPrototypeOf', 'propertyIsEnumerable', 'toLocaleString', 'toString', 'constructor'];
function forEachObjectEnumberableKey(object, fn, bind){
	if (enumerables) for (var i = enumerables.length; i--;){
		var k = enumerables[i];
		// signature has key-value, so overloadSetter can directly pass the
		// method function, without swapping arguments.
		if (hasOwnProperty.call(object, k)) fn.call(bind, k, object[k]);
	}
}
/*</ltIE8>*/

// Function overloading

var Function = this.Function;

Function.prototype.overloadSetter = function(usePlural){
	var self = this;
	return function(a, b){
		if (a == null) return this;
		if (usePlural || typeof a != 'string'){
			for (var k in a) self.call(this, k, a[k]);
			/*<ltIE8>*/
			forEachObjectEnumberableKey(a, self, this);
			/*</ltIE8>*/
		} else {
			self.call(this, a, b);
		}
		return this;
	};
};

Function.prototype.overloadGetter = function(usePlural){
	var self = this;
	return function(a){
		var args, result;
		if (typeof a != 'string') args = a;
		else if (arguments.length > 1) args = arguments;
		else if (usePlural) args = [a];
		if (args){
			result = {};
			for (var i = 0; i < args.length; i++) result[args[i]] = self.call(this, args[i]);
		} else {
			result = self.call(this, a);
		}
		return result;
	};
};

Function.prototype.extend = function(key, value){
	this[key] = value;
}.overloadSetter();

Function.prototype.implement = function(key, value){
	this.prototype[key] = value;
}.overloadSetter();

// From

var slice = Array.prototype.slice;

Array.convert = function(item){
	if (item == null) return [];
	return (Type.isEnumerable(item) && typeof item != 'string') ? (typeOf(item) == 'array') ? item : slice.call(item) : [item];
};

Function.convert = function(item){
	return (typeOf(item) == 'function') ? item : function(){
		return item;
	};
};


Number.convert = function(item){
	var number = parseFloat(item);
	return isFinite(number) ? number : null;
};

String.convert = function(item){
	return item + '';
};



Function.from = Function.convert;
Number.from = Number.convert;
String.from = String.convert;

// hide, protect

Function.implement({

	hide: function(){
		this.$hidden = true;
		return this;
	},

	protect: function(){
		this.$protected = true;
		return this;
	}

});

// Type

var Type = this.Type = function(name, object){
	if (name){
		var lower = name.toLowerCase();
		var typeCheck = function(item){
			return (typeOf(item) == lower);
		};

		Type['is' + name] = typeCheck;
		if (object != null){
			object.prototype.$family = (function(){
				return lower;
			}).hide();
			
		}
	}

	if (object == null) return null;

	object.extend(this);
	object.$constructor = Type;
	object.prototype.$constructor = object;

	return object;
};

var toString = Object.prototype.toString;

Type.isEnumerable = function(item){
	return (item != null && typeof item.length == 'number' && toString.call(item) != '[object Function]' );
};

var hooks = {};

var hooksOf = function(object){
	var type = typeOf(object.prototype);
	return hooks[type] || (hooks[type] = []);
};

var implement = function(name, method){
	if (method && method.$hidden) return;

	var hooks = hooksOf(this);

	for (var i = 0; i < hooks.length; i++){
		var hook = hooks[i];
		if (typeOf(hook) == 'type') implement.call(hook, name, method);
		else hook.call(this, name, method);
	}

	var previous = this.prototype[name];
	if (previous == null || !previous.$protected) this.prototype[name] = method;

	if (this[name] == null && typeOf(method) == 'function') extend.call(this, name, function(item){
		return method.apply(item, slice.call(arguments, 1));
	});
};

var extend = function(name, method){
	if (method && method.$hidden) return;
	var previous = this[name];
	if (previous == null || !previous.$protected) this[name] = method;
};

Type.implement({

	implement: implement.overloadSetter(),

	extend: extend.overloadSetter(),

	alias: function(name, existing){
		implement.call(this, name, this.prototype[existing]);
	}.overloadSetter(),

	mirror: function(hook){
		hooksOf(this).push(hook);
		return this;
	}

});

new Type('Type', Type);

// Default Types

var force = function(name, object, methods){
	var isType = (object != Object),
		prototype = object.prototype;

	if (isType) object = new Type(name, object);

	for (var i = 0, l = methods.length; i < l; i++){
		var key = methods[i],
			generic = object[key],
			proto = prototype[key];

		if (generic) generic.protect();
		if (isType && proto) object.implement(key, proto.protect());
	}

	if (isType){
		var methodsEnumerable = prototype.propertyIsEnumerable(methods[0]);
		object.forEachMethod = function(fn){
			if (!methodsEnumerable) for (var i = 0, l = methods.length; i < l; i++){
				fn.call(prototype, prototype[methods[i]], methods[i]);
			}
			for (var key in prototype) fn.call(prototype, prototype[key], key);
		};
	}

	return force;
};

force('String', String, [
	'charAt', 'charCodeAt', 'concat', 'contains', 'indexOf', 'lastIndexOf', 'match', 'quote', 'replace', 'search',
	'slice', 'split', 'substr', 'substring', 'trim', 'toLowerCase', 'toUpperCase'
])('Array', Array, [
	'pop', 'push', 'reverse', 'shift', 'sort', 'splice', 'unshift', 'concat', 'join', 'slice',
	'indexOf', 'lastIndexOf', 'filter', 'forEach', 'every', 'map', 'some', 'reduce', 'reduceRight', 'contains'
])('Number', Number, [
	'toExponential', 'toFixed', 'toLocaleString', 'toPrecision'
])('Function', Function, [
	'apply', 'call', 'bind'
])('RegExp', RegExp, [
	'exec', 'test'
])('Object', Object, [
	'create', 'defineProperty', 'defineProperties', 'keys',
	'getPrototypeOf', 'getOwnPropertyDescriptor', 'getOwnPropertyNames',
	'preventExtensions', 'isExtensible', 'seal', 'isSealed', 'freeze', 'isFrozen'
])('Date', Date, ['now']);

Object.extend = extend.overloadSetter();

Date.extend('now', function(){
	return +(new Date);
});

new Type('Boolean', Boolean);

// fixes NaN returning as Number

Number.prototype.$family = function(){
	return isFinite(this) ? 'number' : 'null';
}.hide();

// Number.random

Number.extend('random', function(min, max){
	return Math.floor(Math.random() * (max - min + 1) + min);
});

// forEach, each, keys

Array.implement({

	/*<!ES5>*/
	forEach: function(fn, bind){
		for (var i = 0, l = this.length; i < l; i++){
			if (i in this) fn.call(bind, this[i], i, this);
		}
	},
	/*</!ES5>*/

	each: function(fn, bind){
		Array.forEach(this, fn, bind);
		return this;
	}

});

Object.extend({

	keys: function(object){
		var keys = [];
		for (var k in object){
			if (hasOwnProperty.call(object, k)) keys.push(k);
		}
		/*<ltIE8>*/
		forEachObjectEnumberableKey(object, function(k){
			keys.push(k);
		});
		/*</ltIE8>*/
		return keys;
	},

	forEach: function(object, fn, bind){
		Object.keys(object).forEach(function(key){
			fn.call(bind, object[key], key, object);
		});
	}

});

Object.each = Object.forEach;


// Array & Object cloning, Object merging and appending

var cloneOf = function(item){
	switch (typeOf(item)){
		case 'array': return item.clone();
		case 'object': return Object.clone(item);
		default: return item;
	}
};

Array.implement('clone', function(){
	var i = this.length, clone = new Array(i);
	while (i--) clone[i] = cloneOf(this[i]);
	return clone;
});

var mergeOne = function(source, key, current){
	switch (typeOf(current)){
		case 'object':
			if (typeOf(source[key]) == 'object') Object.merge(source[key], current);
			else source[key] = Object.clone(current);
			break;
		case 'array': source[key] = current.clone(); break;
		default: source[key] = current;
	}
	return source;
};

Object.extend({

	merge: function(source, k, v){
		if (typeOf(k) == 'string') return mergeOne(source, k, v);
		for (var i = 1, l = arguments.length; i < l; i++){
			var object = arguments[i];
			for (var key in object) mergeOne(source, key, object[key]);
		}
		return source;
	},

	clone: function(object){
		var clone = {};
		for (var key in object) clone[key] = cloneOf(object[key]);
		return clone;
	},

	append: function(original){
		for (var i = 1, l = arguments.length; i < l; i++){
			var extended = arguments[i] || {};
			for (var key in extended) original[key] = extended[key];
		}
		return original;
	}

});

// Object-less types

['Object', 'WhiteSpace', 'TextNode', 'Collection', 'Arguments'].each(function(name){
	new Type(name);
});

// Unique ID

var UID = Date.now();

String.extend('uniqueID', function(){
	return (UID++).toString(36);
});



})();

/*
---

name: Array

description: Contains Array Prototypes like each, contains, and erase.

license: MIT-style license.

requires: [Type]

provides: Array

...
*/

Array.implement({

	/*<!ES5>*/
	every: function(fn, bind){
		for (var i = 0, l = this.length >>> 0; i < l; i++){
			if ((i in this) && !fn.call(bind, this[i], i, this)) return false;
		}
		return true;
	},

	filter: function(fn, bind){
		var results = [];
		for (var value, i = 0, l = this.length >>> 0; i < l; i++) if (i in this){
			value = this[i];
			if (fn.call(bind, value, i, this)) results.push(value);
		}
		return results;
	},

	indexOf: function(item, from){
		var length = this.length >>> 0;
		for (var i = (from < 0) ? Math.max(0, length + from) : from || 0; i < length; i++){
			if (this[i] === item) return i;
		}
		return -1;
	},

	map: function(fn, bind){
		var length = this.length >>> 0, results = Array(length);
		for (var i = 0; i < length; i++){
			if (i in this) results[i] = fn.call(bind, this[i], i, this);
		}
		return results;
	},

	some: function(fn, bind){
		for (var i = 0, l = this.length >>> 0; i < l; i++){
			if ((i in this) && fn.call(bind, this[i], i, this)) return true;
		}
		return false;
	},
	/*</!ES5>*/

	clean: function(){
		return this.filter(function(item){
			return item != null;
		});
	},

	invoke: function(methodName){
		var args = Array.slice(arguments, 1);
		return this.map(function(item){
			return item[methodName].apply(item, args);
		});
	},

	associate: function(keys){
		var obj = {}, length = Math.min(this.length, keys.length);
		for (var i = 0; i < length; i++) obj[keys[i]] = this[i];
		return obj;
	},

	link: function(object){
		var result = {};
		for (var i = 0, l = this.length; i < l; i++){
			for (var key in object){
				if (object[key](this[i])){
					result[key] = this[i];
					delete object[key];
					break;
				}
			}
		}
		return result;
	},

	contains: function(item, from){
		return this.indexOf(item, from) != -1;
	},

	append: function(array){
		this.push.apply(this, array);
		return this;
	},

	getLast: function(){
		return (this.length) ? this[this.length - 1] : null;
	},

	getRandom: function(){
		return (this.length) ? this[Number.random(0, this.length - 1)] : null;
	},

	include: function(item){
		if (!this.contains(item)) this.push(item);
		return this;
	},

	combine: function(array){
		for (var i = 0, l = array.length; i < l; i++) this.include(array[i]);
		return this;
	},

	erase: function(item){
		for (var i = this.length; i--;){
			if (this[i] === item) this.splice(i, 1);
		}
		return this;
	},

	empty: function(){
		this.length = 0;
		return this;
	},

	flatten: function(){
		var array = [];
		for (var i = 0, l = this.length; i < l; i++){
			var type = typeOf(this[i]);
			if (type == 'null') continue;
			array = array.concat((type == 'array' || type == 'collection' || type == 'arguments' || instanceOf(this[i], Array)) ? Array.flatten(this[i]) : this[i]);
		}
		return array;
	},

	pick: function(){
		for (var i = 0, l = this.length; i < l; i++){
			if (this[i] != null) return this[i];
		}
		return null;
	},

	hexToRgb: function(array){
		if (this.length != 3) return null;
		var rgb = this.map(function(value){
			if (value.length == 1) value += value;
			return parseInt(value, 16);
		});
		return (array) ? rgb : 'rgb(' + rgb + ')';
	},

	rgbToHex: function(array){
		if (this.length < 3) return null;
		if (this.length == 4 && this[3] == 0 && !array) return 'transparent';
		var hex = [];
		for (var i = 0; i < 3; i++){
			var bit = (this[i] - 0).toString(16);
			hex.push((bit.length == 1) ? '0' + bit : bit);
		}
		return (array) ? hex : '#' + hex.join('');
	}

});



/*
---

name: Function

description: Contains Function Prototypes like create, bind, pass, and delay.

license: MIT-style license.

requires: Type

provides: Function

...
*/

Function.extend({

	attempt: function(){
		for (var i = 0, l = arguments.length; i < l; i++){
			try {
				return arguments[i]();
			} catch (e){}
		}
		return null;
	}

});

Function.implement({

	attempt: function(args, bind){
		try {
			return this.apply(bind, Array.convert(args));
		} catch (e){}

		return null;
	},

	/*<!ES5-bind>*/
	bind: function(that){
		var self = this,
			args = arguments.length > 1 ? Array.slice(arguments, 1) : null,
			F = function(){};

		var bound = function(){
			var context = that, length = arguments.length;
			if (this instanceof bound){
				F.prototype = self.prototype;
				context = new F;
			}
			var result = (!args && !length)
				? self.call(context)
				: self.apply(context, args && length ? args.concat(Array.slice(arguments)) : args || arguments);
			return context == that ? result : context;
		};
		return bound;
	},
	/*</!ES5-bind>*/

	pass: function(args, bind){
		var self = this;
		if (args != null) args = Array.convert(args);
		return function(){
			return self.apply(bind, args || arguments);
		};
	},

	delay: function(delay, bind, args){
		return setTimeout(this.pass((args == null ? [] : args), bind), delay);
	},

	periodical: function(periodical, bind, args){
		return setInterval(this.pass((args == null ? [] : args), bind), periodical);
	}

});



/*
---

name: Number

description: Contains Number Prototypes like limit, round, times, and ceil.

license: MIT-style license.

requires: Type

provides: Number

...
*/

Number.implement({

	limit: function(min, max){
		return Math.min(max, Math.max(min, this));
	},

	round: function(precision){
		precision = Math.pow(10, precision || 0).toFixed(precision < 0 ? -precision : 0);
		return Math.round(this * precision) / precision;
	},

	times: function(fn, bind){
		for (var i = 0; i < this; i++) fn.call(bind, i, this);
	},

	toFloat: function(){
		return parseFloat(this);
	},

	toInt: function(base){
		return parseInt(this, base || 10);
	}

});

Number.alias('each', 'times');

(function(math){

var methods = {};

math.each(function(name){
	if (!Number[name]) methods[name] = function(){
		return Math[name].apply(null, [this].concat(Array.convert(arguments)));
	};
});

Number.implement(methods);

})(['abs', 'acos', 'asin', 'atan', 'atan2', 'ceil', 'cos', 'exp', 'floor', 'log', 'max', 'min', 'pow', 'sin', 'sqrt', 'tan']);

/*
---

name: String

description: Contains String Prototypes like camelCase, capitalize, test, and toInt.

license: MIT-style license.

requires: [Type, Array]

provides: String

...
*/

String.implement({

	//<!ES6>
	contains: function(string, index){
		return (index ? String(this).slice(index) : String(this)).indexOf(string) > -1;
	},
	//</!ES6>

	test: function(regex, params){
		return ((typeOf(regex) == 'regexp') ? regex : new RegExp('' + regex, params)).test(this);
	},

	trim: function(){
		return String(this).replace(/^\s+|\s+$/g, '');
	},

	clean: function(){
		return String(this).replace(/\s+/g, ' ').trim();
	},

	camelCase: function(){
		return String(this).replace(/-\D/g, function(match){
			return match.charAt(1).toUpperCase();
		});
	},

	hyphenate: function(){
		return String(this).replace(/[A-Z]/g, function(match){
			return ('-' + match.charAt(0).toLowerCase());
		});
	},

	capitalize: function(){
		return String(this).replace(/\b[a-z]/g, function(match){
			return match.toUpperCase();
		});
	},

	escapeRegExp: function(){
		return String(this).replace(/([-.*+?^${}()|[\]\/\\])/g, '\\$1');
	},

	toInt: function(base){
		return parseInt(this, base || 10);
	},

	toFloat: function(){
		return parseFloat(this);
	},

	hexToRgb: function(array){
		var hex = String(this).match(/^#?(\w{1,2})(\w{1,2})(\w{1,2})$/);
		return (hex) ? hex.slice(1).hexToRgb(array) : null;
	},

	rgbToHex: function(array){
		var rgb = String(this).match(/\d{1,3}/g);
		return (rgb) ? rgb.rgbToHex(array) : null;
	},

	substitute: function(object, regexp){
		return String(this).replace(regexp || (/\\?\{([^{}]+)\}/g), function(match, name){
			if (match.charAt(0) == '\\') return match.slice(1);
			return (object[name] != null) ? object[name] : '';
		});
	}

});



/*
---

name: Browser

description: The Browser Object. Contains Browser initialization, Window and Document, and the Browser Hash.

license: MIT-style license.

requires: [Array, Function, Number, String]

provides: [Browser, Window, Document]

...
*/

(function(){

var document = this.document;
var window = document.window = this;

var parse = function(ua, platform){
	ua = ua.toLowerCase();
	platform = (platform ? platform.toLowerCase() : '');

	// chrome is included in the edge UA, so need to check for edge first,
	// before checking if it's chrome.
	var UA = ua.match(/(edge)[\s\/:]([\w\d\.]+)/);
	if (!UA){
		UA = ua.match(/(opera|ie|firefox|chrome|trident|crios|version)[\s\/:]([\w\d\.]+)?.*?(safari|(?:rv[\s\/:]|version[\s\/:])([\w\d\.]+)|$)/) || [null, 'unknown', 0];
	}

	if (UA[1] == 'trident'){
		UA[1] = 'ie';
		if (UA[4]) UA[2] = UA[4];
	} else if (UA[1] == 'crios'){
		UA[1] = 'chrome';
	}

	platform = ua.match(/ip(?:ad|od|hone)/) ? 'ios' : (ua.match(/(?:webos|android)/) || ua.match(/mac|win|linux/) || ['other'])[0];
	if (platform == 'win') platform = 'windows';

	return {
		extend: Function.prototype.extend,
		name: (UA[1] == 'version') ? UA[3] : UA[1],
		version: parseFloat((UA[1] == 'opera' && UA[4]) ? UA[4] : UA[2]),
		platform: platform
	};
};

var Browser = this.Browser = parse(navigator.userAgent, navigator.platform);

if (Browser.name == 'ie' && document.documentMode){
	Browser.version = document.documentMode;
}

Browser.extend({
	Features: {
		xpath: !!(document.evaluate),
		air: !!(window.runtime),
		query: !!(document.querySelector),
		json: !!(window.JSON)
	},
	parseUA: parse
});



// Request

Browser.Request = (function(){

	var XMLHTTP = function(){
		return new XMLHttpRequest();
	};

	var MSXML2 = function(){
		return new ActiveXObject('MSXML2.XMLHTTP');
	};

	var MSXML = function(){
		return new ActiveXObject('Microsoft.XMLHTTP');
	};

	return Function.attempt(function(){
		XMLHTTP();
		return XMLHTTP;
	}, function(){
		MSXML2();
		return MSXML2;
	}, function(){
		MSXML();
		return MSXML;
	});

})();

Browser.Features.xhr = !!(Browser.Request);



// String scripts

Browser.exec = function(text){
	if (!text) return text;
	if (window.execScript){
		window.execScript(text);
	} else {
		var script = document.createElement('script');
		script.setAttribute('type', 'text/javascript');
		script.text = text;
		document.head.appendChild(script);
		document.head.removeChild(script);
	}
	return text;
};

String.implement('stripScripts', function(exec){
	var scripts = '';
	var text = this.replace(/<script[^>]*>([\s\S]*?)<\/script>/gi, function(all, code){
		scripts += code + '\n';
		return '';
	});
	if (exec === true) Browser.exec(scripts);
	else if (typeOf(exec) == 'function') exec(scripts, text);
	return text;
});

// Window, Document

Browser.extend({
	Document: this.Document,
	Window: this.Window,
	Element: this.Element,
	Event: this.Event
});

this.Window = this.$constructor = new Type('Window', function(){});

this.$family = Function.convert('window').hide();

Window.mirror(function(name, method){
	window[name] = method;
});

this.Document = document.$constructor = new Type('Document', function(){});

document.$family = Function.convert('document').hide();

Document.mirror(function(name, method){
	document[name] = method;
});

document.html = document.documentElement;
if (!document.head) document.head = document.getElementsByTagName('head')[0];

if (document.execCommand) try {
	document.execCommand('BackgroundImageCache', false, true);
} catch (e){}

/*<ltIE9>*/
if (this.attachEvent && !this.addEventListener){
	var unloadEvent = function(){
		this.detachEvent('onunload', unloadEvent);
		document.head = document.html = document.window = null;
		window = this.Window = document = null;
	};
	this.attachEvent('onunload', unloadEvent);
}

// IE fails on collections and <select>.options (refers to <select>)
var arrayFrom = Array.convert;
try {
	arrayFrom(document.html.childNodes);
} catch (e){
	Array.convert = function(item){
		if (typeof item != 'string' && Type.isEnumerable(item) && typeOf(item) != 'array'){
			var i = item.length, array = new Array(i);
			while (i--) array[i] = item[i];
			return array;
		}
		return arrayFrom(item);
	};

	var prototype = Array.prototype,
		slice = prototype.slice;
	['pop', 'push', 'reverse', 'shift', 'sort', 'splice', 'unshift', 'concat', 'join', 'slice'].each(function(name){
		var method = prototype[name];
		Array[name] = function(item){
			return method.apply(Array.convert(item), slice.call(arguments, 1));
		};
	});
}
/*</ltIE9>*/



})();

/*
---

name: Class

description: Contains the Class Function for easily creating, extending, and implementing reusable Classes.

license: MIT-style license.

requires: [Array, String, Function, Number]

provides: Class

...
*/

(function(){

var Class = this.Class = new Type('Class', function(params){
	if (instanceOf(params, Function)) params = {initialize: params};

	var newClass = function(){
		reset(this);
		if (newClass.$prototyping) return this;
		this.$caller = null;
		this.$family = null;
		var value = (this.initialize) ? this.initialize.apply(this, arguments) : this;
		this.$caller = this.caller = null;
		return value;
	}.extend(this).implement(params);

	newClass.$constructor = Class;
	newClass.prototype.$constructor = newClass;
	newClass.prototype.parent = parent;

	return newClass;
});

var parent = function(){
	if (!this.$caller) throw new Error('The method "parent" cannot be called.');
	var name = this.$caller.$name,
		parent = this.$caller.$owner.parent,
		previous = (parent) ? parent.prototype[name] : null;
	if (!previous) throw new Error('The method "' + name + '" has no parent.');
	return previous.apply(this, arguments);
};

var reset = function(object){
	for (var key in object){
		var value = object[key];
		switch (typeOf(value)){
			case 'object':
				var F = function(){};
				F.prototype = value;
				object[key] = reset(new F);
				break;
			case 'array': object[key] = value.clone(); break;
		}
	}
	return object;
};

var wrap = function(self, key, method){
	if (method.$origin) method = method.$origin;
	var wrapper = function(){
		if (method.$protected && this.$caller == null) throw new Error('The method "' + key + '" cannot be called.');
		var caller = this.caller, current = this.$caller;
		this.caller = current; this.$caller = wrapper;
		var result = method.apply(this, arguments);
		this.$caller = current; this.caller = caller;
		return result;
	}.extend({$owner: self, $origin: method, $name: key});
	return wrapper;
};

var implement = function(key, value, retain){
	if (Class.Mutators.hasOwnProperty(key)){
		value = Class.Mutators[key].call(this, value);
		if (value == null) return this;
	}

	if (typeOf(value) == 'function'){
		if (value.$hidden) return this;
		this.prototype[key] = (retain) ? value : wrap(this, key, value);
	} else {
		Object.merge(this.prototype, key, value);
	}

	return this;
};

var getInstance = function(klass){
	klass.$prototyping = true;
	var proto = new klass;
	delete klass.$prototyping;
	return proto;
};

Class.implement('implement', implement.overloadSetter());

Class.Mutators = {

	Extends: function(parent){
		this.parent = parent;
		this.prototype = getInstance(parent);
	},

	Implements: function(items){
		Array.convert(items).each(function(item){
			var instance = new item;
			for (var key in instance) implement.call(this, key, instance[key], true);
		}, this);
	}
};

})();

/*
---

name: Class.Extras

description: Contains Utility Classes that can be implemented into your own Classes to ease the execution of many common tasks.

license: MIT-style license.

requires: Class

provides: [Class.Extras, Chain, Events, Options]

...
*/

(function(){

this.Chain = new Class({

	$chain: [],

	chain: function(){
		this.$chain.append(Array.flatten(arguments));
		return this;
	},

	callChain: function(){
		return (this.$chain.length) ? this.$chain.shift().apply(this, arguments) : false;
	},

	clearChain: function(){
		this.$chain.empty();
		return this;
	}

});

var removeOn = function(string){
	return string.replace(/^on([A-Z])/, function(full, first){
		return first.toLowerCase();
	});
};

this.Events = new Class({

	$events: {},

	addEvent: function(type, fn, internal){
		type = removeOn(type);

		

		this.$events[type] = (this.$events[type] || []).include(fn);
		if (internal) fn.internal = true;
		return this;
	},

	addEvents: function(events){
		for (var type in events) this.addEvent(type, events[type]);
		return this;
	},

	fireEvent: function(type, args, delay){
		type = removeOn(type);
		var events = this.$events[type];
		if (!events) return this;
		args = Array.convert(args);
		events.each(function(fn){
			if (delay) fn.delay(delay, this, args);
			else fn.apply(this, args);
		}, this);
		return this;
	},

	removeEvent: function(type, fn){
		type = removeOn(type);
		var events = this.$events[type];
		if (events && !fn.internal){
			var index = events.indexOf(fn);
			if (index != -1) delete events[index];
		}
		return this;
	},

	removeEvents: function(events){
		var type;
		if (typeOf(events) == 'object'){
			for (type in events) this.removeEvent(type, events[type]);
			return this;
		}
		if (events) events = removeOn(events);
		for (type in this.$events){
			if (events && events != type) continue;
			var fns = this.$events[type];
			for (var i = fns.length; i--;) if (i in fns){
				this.removeEvent(type, fns[i]);
			}
		}
		return this;
	}

});

this.Options = new Class({

	setOptions: function(){
		var options = this.options = Object.merge.apply(null, [{}, this.options].append(arguments));
		if (this.addEvent) for (var option in options){
			if (typeOf(options[option]) != 'function' || !(/^on[A-Z]/).test(option)) continue;
			this.addEvent(option, options[option]);
			delete options[option];
		}
		return this;
	}

});

})();

/*
---

name: Class.Thenable

description: Contains a Utility Class that can be implemented into your own Classes to make them "thenable".

license: MIT-style license.

requires: Class

provides: [Class.Thenable]

...
*/

(function(){

var STATE_PENDING = 0,
	STATE_FULFILLED = 1,
	STATE_REJECTED = 2;

var Thenable = Class.Thenable = new Class({

	$thenableState: STATE_PENDING,
	$thenableResult: null,
	$thenableReactions: [],

	resolve: function(value){
		resolve(this, value);
		return this;
	},

	reject: function(reason){
		reject(this, reason);
		return this;
	},

	getThenableState: function(){
		switch (this.$thenableState){
			case STATE_PENDING:
				return 'pending';

			case STATE_FULFILLED:
				return 'fulfilled';

			case STATE_REJECTED:
				return 'rejected';
		}
	},

	resetThenable: function(reason){
		reject(this, reason);
		reset(this);
		return this;
	},

	then: function(onFulfilled, onRejected){
		if (typeof onFulfilled !== 'function') onFulfilled = 'Identity';
		if (typeof onRejected !== 'function') onRejected = 'Thrower';

		var thenable = new Thenable();

		this.$thenableReactions.push({
			thenable: thenable,
			fulfillHandler: onFulfilled,
			rejectHandler: onRejected
		});

		if (this.$thenableState !== STATE_PENDING){
			react(this);
		}

		return thenable;
	},

	'catch': function(onRejected){
		return this.then(null, onRejected);
	}

});

Thenable.extend({
	resolve: function(value){
		var thenable;
		if (value instanceof Thenable){
			thenable = value;
		} else {
			thenable = new Thenable();
			resolve(thenable, value);
		}
		return thenable;
	},
	reject: function(reason){
		var thenable = new Thenable();
		reject(thenable, reason);
		return thenable;
	}
});

// Private functions

function resolve(thenable, value){
	if (thenable.$thenableState === STATE_PENDING){
		if (thenable === value){
			reject(thenable, new TypeError('Tried to resolve a thenable with itself.'));
		} else if (value && (typeof value === 'object' || typeof value === 'function')){
			var then;
			try {
				then = value.then;
			} catch (exception){
				reject(thenable, exception);
			}
			if (typeof then === 'function'){
				var resolved = false;
				defer(function(){
					try {
						then.call(
							value,
							function(nextValue){
								if (!resolved){
									resolved = true;
									resolve(thenable, nextValue);
								}
							},
							function(reason){
								if (!resolved){
									resolved = true;
									reject(thenable, reason);
								}
							}
						);
					} catch (exception){
						if (!resolved){
							resolved = true;
							reject(thenable, exception);
						}
					}
				});
			} else {
				fulfill(thenable, value);
			}
		} else {
			fulfill(thenable, value);
		}
	}
}

function fulfill(thenable, value){
	if (thenable.$thenableState === STATE_PENDING){
		thenable.$thenableResult = value;
		thenable.$thenableState = STATE_FULFILLED;

		react(thenable);
	}
}

function reject(thenable, reason){
	if (thenable.$thenableState === STATE_PENDING){
		thenable.$thenableResult = reason;
		thenable.$thenableState = STATE_REJECTED;

		react(thenable);
	}
}

function reset(thenable){
	if (thenable.$thenableState !== STATE_PENDING){
		thenable.$thenableResult = null;
		thenable.$thenableState = STATE_PENDING;
	}
}

function react(thenable){
	var state = thenable.$thenableState,
		result = thenable.$thenableResult,
		reactions = thenable.$thenableReactions,
		type;

	if (state === STATE_FULFILLED){
		thenable.$thenableReactions = [];
		type = 'fulfillHandler';
	} else if (state == STATE_REJECTED){
		thenable.$thenableReactions = [];
		type = 'rejectHandler';
	}

	if (type){
		defer(handle.pass([result, reactions, type]));
	}
}

function handle(result, reactions, type){
	for (var i = 0, l = reactions.length; i < l; ++i){
		var reaction = reactions[i],
			handler = reaction[type];

		if (handler === 'Identity'){
			resolve(reaction.thenable, result);
		} else if (handler === 'Thrower'){
			reject(reaction.thenable, result);
		} else {
			try {
				resolve(reaction.thenable, handler(result));
			} catch (exception){
				reject(reaction.thenable, exception);
			}
		}
	}
}

var defer;
if (typeof process !== 'undefined' && typeof process.nextTick === 'function'){
	defer = process.nextTick;
} else if (typeof setImmediate !== 'undefined'){
	defer = setImmediate;
} else {
	defer = function(fn){
		setTimeout(fn, 0);
	};
}

})();

/*
---

name: Object

description: Object generic methods

license: MIT-style license.

requires: Type

provides: [Object, Hash]

...
*/

(function(){

Object.extend({

	subset: function(object, keys){
		var results = {};
		for (var i = 0, l = keys.length; i < l; i++){
			var k = keys[i];
			if (k in object) results[k] = object[k];
		}
		return results;
	},

	map: function(object, fn, bind){
		var results = {};
		var keys = Object.keys(object);
		for (var i = 0; i < keys.length; i++){
			var key = keys[i];
			results[key] = fn.call(bind, object[key], key, object);
		}
		return results;
	},

	filter: function(object, fn, bind){
		var results = {};
		var keys = Object.keys(object);
		for (var i = 0; i < keys.length; i++){
			var key = keys[i], value = object[key];
			if (fn.call(bind, value, key, object)) results[key] = value;
		}
		return results;
	},

	every: function(object, fn, bind){
		var keys = Object.keys(object);
		for (var i = 0; i < keys.length; i++){
			var key = keys[i];
			if (!fn.call(bind, object[key], key)) return false;
		}
		return true;
	},

	some: function(object, fn, bind){
		var keys = Object.keys(object);
		for (var i = 0; i < keys.length; i++){
			var key = keys[i];
			if (fn.call(bind, object[key], key)) return true;
		}
		return false;
	},

	values: function(object){
		var values = [];
		var keys = Object.keys(object);
		for (var i = 0; i < keys.length; i++){
			var k = keys[i];
			values.push(object[k]);
		}
		return values;
	},

	getLength: function(object){
		return Object.keys(object).length;
	},

	keyOf: function(object, value){
		var keys = Object.keys(object);
		for (var i = 0; i < keys.length; i++){
			var key = keys[i];
			if (object[key] === value) return key;
		}
		return null;
	},

	contains: function(object, value){
		return Object.keyOf(object, value) != null;
	},

	toQueryString: function(object, base){
		var queryString = [];

		Object.each(object, function(value, key){
			if (base) key = base + '[' + key + ']';
			var result;
			switch (typeOf(value)){
				case 'object': result = Object.toQueryString(value, key); break;
				case 'array':
					var qs = {};
					value.each(function(val, i){
						qs[i] = val;
					});
					result = Object.toQueryString(qs, key);
					break;
				default: result = key + '=' + encodeURIComponent(value);
			}
			if (value != null) queryString.push(result);
		});

		return queryString.join('&');
	}

});

})();



/*
---
name: Slick.Parser
description: Standalone CSS3 Selector parser
provides: Slick.Parser
...
*/

;(function(){

var parsed,
	separatorIndex,
	combinatorIndex,
	reversed,
	cache = {},
	reverseCache = {},
	reUnescape = /\\/g;

var parse = function(expression, isReversed){
	if (expression == null) return null;
	if (expression.Slick === true) return expression;
	expression = ('' + expression).replace(/^\s+|\s+$/g, '');
	reversed = !!isReversed;
	var currentCache = (reversed) ? reverseCache : cache;
	if (currentCache[expression]) return currentCache[expression];
	parsed = {
		Slick: true,
		expressions: [],
		raw: expression,
		reverse: function(){
			return parse(this.raw, true);
		}
	};
	separatorIndex = -1;
	while (expression != (expression = expression.replace(regexp, parser)));
	parsed.length = parsed.expressions.length;
	return currentCache[parsed.raw] = (reversed) ? reverse(parsed) : parsed;
};

var reverseCombinator = function(combinator){
	if (combinator === '!') return ' ';
	else if (combinator === ' ') return '!';
	else if ((/^!/).test(combinator)) return combinator.replace(/^!/, '');
	else return '!' + combinator;
};

var reverse = function(expression){
	var expressions = expression.expressions;
	for (var i = 0; i < expressions.length; i++){
		var exp = expressions[i];
		var last = {parts: [], tag: '*', combinator: reverseCombinator(exp[0].combinator)};

		for (var j = 0; j < exp.length; j++){
			var cexp = exp[j];
			if (!cexp.reverseCombinator) cexp.reverseCombinator = ' ';
			cexp.combinator = cexp.reverseCombinator;
			delete cexp.reverseCombinator;
		}

		exp.reverse().push(last);
	}
	return expression;
};

var escapeRegExp = function(string){// Credit: XRegExp 0.6.1 (c) 2007-2008 Steven Levithan <http://stevenlevithan.com/regex/xregexp/> MIT License
	return string.replace(/[-[\]{}()*+?.\\^$|,#\s]/g, function(match){
		return '\\' + match;
	});
};

var regexp = new RegExp(
/*
#!/usr/bin/env ruby
puts "\t\t" + DATA.read.gsub(/\(\?x\)|\s+#.*$|\s+|\\$|\\n/,'')
__END__
	"(?x)^(?:\
	  \\s* ( , ) \\s*               # Separator          \n\
	| \\s* ( <combinator>+ ) \\s*   # Combinator         \n\
	|      ( \\s+ )                 # CombinatorChildren \n\
	|      ( <unicode>+ | \\* )     # Tag                \n\
	| \\#  ( <unicode>+       )     # ID                 \n\
	| \\.  ( <unicode>+       )     # ClassName          \n\
	|                               # Attribute          \n\
	\\[  \
		\\s* (<unicode1>+)  (?:  \
			\\s* ([*^$!~|]?=)  (?:  \
				\\s* (?:\
					([\"']?)(.*?)\\9 \
				)\
			)  \
		)?  \\s*  \
	\\](?!\\]) \n\
	|   :+ ( <unicode>+ )(?:\
	\\( (?:\
		(?:([\"'])([^\\12]*)\\12)|((?:\\([^)]+\\)|[^()]*)+)\
	) \\)\
	)?\
	)"
*/
	"^(?:\\s*(,)\\s*|\\s*(<combinator>+)\\s*|(\\s+)|(<unicode>+|\\*)|\\#(<unicode>+)|\\.(<unicode>+)|\\[\\s*(<unicode1>+)(?:\\s*([*^$!~|]?=)(?:\\s*(?:([\"']?)(.*?)\\9)))?\\s*\\](?!\\])|(:+)(<unicode>+)(?:\\((?:(?:([\"'])([^\\13]*)\\13)|((?:\\([^)]+\\)|[^()]*)+))\\))?)"
	.replace(/<combinator>/, '[' + escapeRegExp('>+~`!@$%^&={}\\;</') + ']')
	.replace(/<unicode>/g, '(?:[\\w\\u00a1-\\uFFFF-]|\\\\[^\\s0-9a-f])')
	.replace(/<unicode1>/g, '(?:[:\\w\\u00a1-\\uFFFF-]|\\\\[^\\s0-9a-f])')
);

function parser(
	rawMatch,

	separator,
	combinator,
	combinatorChildren,

	tagName,
	id,
	className,

	attributeKey,
	attributeOperator,
	attributeQuote,
	attributeValue,

	pseudoMarker,
	pseudoClass,
	pseudoQuote,
	pseudoClassQuotedValue,
	pseudoClassValue
){
	if (separator || separatorIndex === -1){
		parsed.expressions[++separatorIndex] = [];
		combinatorIndex = -1;
		if (separator) return '';
	}

	if (combinator || combinatorChildren || combinatorIndex === -1){
		combinator = combinator || ' ';
		var currentSeparator = parsed.expressions[separatorIndex];
		if (reversed && currentSeparator[combinatorIndex])
			currentSeparator[combinatorIndex].reverseCombinator = reverseCombinator(combinator);
		currentSeparator[++combinatorIndex] = {combinator: combinator, tag: '*'};
	}

	var currentParsed = parsed.expressions[separatorIndex][combinatorIndex];

	if (tagName){
		currentParsed.tag = tagName.replace(reUnescape, '');

	} else if (id){
		currentParsed.id = id.replace(reUnescape, '');

	} else if (className){
		className = className.replace(reUnescape, '');

		if (!currentParsed.classList) currentParsed.classList = [];
		if (!currentParsed.classes) currentParsed.classes = [];
		currentParsed.classList.push(className);
		currentParsed.classes.push({
			value: className,
			regexp: new RegExp('(^|\\s)' + escapeRegExp(className) + '(\\s|$)')
		});

	} else if (pseudoClass){
		pseudoClassValue = pseudoClassValue || pseudoClassQuotedValue;
		pseudoClassValue = pseudoClassValue ? pseudoClassValue.replace(reUnescape, '') : null;

		if (!currentParsed.pseudos) currentParsed.pseudos = [];
		currentParsed.pseudos.push({
			key: pseudoClass.replace(reUnescape, ''),
			value: pseudoClassValue,
			type: pseudoMarker.length == 1 ? 'class' : 'element'
		});

	} else if (attributeKey){
		attributeKey = attributeKey.replace(reUnescape, '');
		attributeValue = (attributeValue || '').replace(reUnescape, '');

		var test, regexp;

		switch (attributeOperator){
			case '^=' : regexp = new RegExp(       '^'+ escapeRegExp(attributeValue)            ); break;
			case '$=' : regexp = new RegExp(            escapeRegExp(attributeValue) +'$'       ); break;
			case '~=' : regexp = new RegExp( '(^|\\s)'+ escapeRegExp(attributeValue) +'(\\s|$)' ); break;
			case '|=' : regexp = new RegExp(       '^'+ escapeRegExp(attributeValue) +'(-|$)'   ); break;
			case  '=' : test = function(value){
				return attributeValue == value;
			}; break;
			case '*=' : test = function(value){
				return value && value.indexOf(attributeValue) > -1;
			}; break;
			case '!=' : test = function(value){
				return attributeValue != value;
			}; break;
			default   : test = function(value){
				return !!value;
			};
		}

		if (attributeValue == '' && (/^[*$^]=$/).test(attributeOperator)) test = function(){
			return false;
		};

		if (!test) test = function(value){
			return value && regexp.test(value);
		};

		if (!currentParsed.attributes) currentParsed.attributes = [];
		currentParsed.attributes.push({
			key: attributeKey,
			operator: attributeOperator,
			value: attributeValue,
			test: test
		});

	}

	return '';
};

// Slick NS

var Slick = (this.Slick || {});

Slick.parse = function(expression){
	return parse(expression);
};

Slick.escapeRegExp = escapeRegExp;

if (!this.Slick) this.Slick = Slick;

}).apply(/*<CommonJS>*/(typeof exports != 'undefined') ? exports : /*</CommonJS>*/this);

/*
---
name: Slick.Finder
description: The new, superfast css selector engine.
provides: Slick.Finder
requires: Slick.Parser
...
*/

;(function(){

var local = {},
	featuresCache = {},
	toString = Object.prototype.toString;

// Feature / Bug detection

local.isNativeCode = function(fn){
	return (/\{\s*\[native code\]\s*\}/).test('' + fn);
};

local.isXML = function(document){
	return (!!document.xmlVersion) || (!!document.xml) || (toString.call(document) == '[object XMLDocument]') ||
	(document.nodeType == 9 && document.documentElement.nodeName != 'HTML');
};

local.setDocument = function(document){

	// convert elements / window arguments to document. if document cannot be extrapolated, the function returns.
	var nodeType = document.nodeType;
	if (nodeType == 9); // document
	else if (nodeType) document = document.ownerDocument; // node
	else if (document.navigator) document = document.document; // window
	else return;

	// check if it's the old document

	if (this.document === document) return;
	this.document = document;

	// check if we have done feature detection on this document before

	var root = document.documentElement,
		rootUid = this.getUIDXML(root),
		features = featuresCache[rootUid],
		feature;

	if (features){
		for (feature in features){
			this[feature] = features[feature];
		}
		return;
	}

	features = featuresCache[rootUid] = {};

	features.root = root;
	features.isXMLDocument = this.isXML(document);

	features.brokenStarGEBTN
	= features.starSelectsClosedQSA
	= features.idGetsName
	= features.brokenMixedCaseQSA
	= features.brokenGEBCN
	= features.brokenCheckedQSA
	= features.brokenEmptyAttributeQSA
	= features.isHTMLDocument
	= features.nativeMatchesSelector
	= false;

	var starSelectsClosed, starSelectsComments,
		brokenSecondClassNameGEBCN, cachedGetElementsByClassName,
		brokenFormAttributeGetter;

	var selected, id = 'slick_uniqueid';
	var testNode = document.createElement('div');

	var testRoot = document.body || document.getElementsByTagName('body')[0] || root;
	testRoot.appendChild(testNode);

	// on non-HTML documents innerHTML and getElementsById doesnt work properly
	try {
		testNode.innerHTML = '<a id="'+id+'"></a>';
		features.isHTMLDocument = !!document.getElementById(id);
	} catch (e){}

	if (features.isHTMLDocument){

		testNode.style.display = 'none';

		// IE returns comment nodes for getElementsByTagName('*') for some documents
		testNode.appendChild(document.createComment(''));
		starSelectsComments = (testNode.getElementsByTagName('*').length > 1);

		// IE returns closed nodes (EG:"</foo>") for getElementsByTagName('*') for some documents
		try {
			testNode.innerHTML = 'foo</foo>';
			selected = testNode.getElementsByTagName('*');
			starSelectsClosed = (selected && !!selected.length && selected[0].nodeName.charAt(0) == '/');
		} catch (e){};

		features.brokenStarGEBTN = starSelectsComments || starSelectsClosed;

		// IE returns elements with the name instead of just id for getElementsById for some documents
		try {
			testNode.innerHTML = '<a name="'+ id +'"></a><b id="'+ id +'"></b>';
			features.idGetsName = document.getElementById(id) === testNode.firstChild;
		} catch (e){}

		if (testNode.getElementsByClassName){

			// Safari 3.2 getElementsByClassName caches results
			try {
				testNode.innerHTML = '<a class="f"></a><a class="b"></a>';
				testNode.getElementsByClassName('b').length;
				testNode.firstChild.className = 'b';
				cachedGetElementsByClassName = (testNode.getElementsByClassName('b').length != 2);
			} catch (e){};

			// Opera 9.6 getElementsByClassName doesnt detects the class if its not the first one
			try {
				testNode.innerHTML = '<a class="a"></a><a class="f b a"></a>';
				brokenSecondClassNameGEBCN = (testNode.getElementsByClassName('a').length != 2);
			} catch (e){}

			features.brokenGEBCN = cachedGetElementsByClassName || brokenSecondClassNameGEBCN;
		}

		if (testNode.querySelectorAll){
			// IE 8 returns closed nodes (EG:"</foo>") for querySelectorAll('*') for some documents
			try {
				testNode.innerHTML = 'foo</foo>';
				selected = testNode.querySelectorAll('*');
				features.starSelectsClosedQSA = (selected && !!selected.length && selected[0].nodeName.charAt(0) == '/');
			} catch (e){}

			// Safari 3.2 querySelectorAll doesnt work with mixedcase on quirksmode
			try {
				testNode.innerHTML = '<a class="MiX"></a>';
				features.brokenMixedCaseQSA = !testNode.querySelectorAll('.MiX').length;
			} catch (e){}

			// Webkit and Opera dont return selected options on querySelectorAll
			try {
				testNode.innerHTML = '<select><option selected="selected">a</option></select>';
				features.brokenCheckedQSA = (testNode.querySelectorAll(':checked').length == 0);
			} catch (e){};

			// IE returns incorrect results for attr[*^$]="" selectors on querySelectorAll
			try {
				testNode.innerHTML = '<a class=""></a>';
				features.brokenEmptyAttributeQSA = (testNode.querySelectorAll('[class*=""]').length != 0);
			} catch (e){}

		}

		// IE6-7, if a form has an input of id x, form.getAttribute(x) returns a reference to the input
		try {
			testNode.innerHTML = '<form action="s"><input id="action"/></form>';
			brokenFormAttributeGetter = (testNode.firstChild.getAttribute('action') != 's');
		} catch (e){}

		// native matchesSelector function

		features.nativeMatchesSelector = root.matches || /*root.msMatchesSelector ||*/ root.mozMatchesSelector || root.webkitMatchesSelector;
		if (features.nativeMatchesSelector) try {
			// if matchesSelector trows errors on incorrect sintaxes we can use it
			features.nativeMatchesSelector.call(root, ':slick');
			features.nativeMatchesSelector = null;
		} catch (e){}

	}

	try {
		root.slick_expando = 1;
		delete root.slick_expando;
		features.getUID = this.getUIDHTML;
	} catch (e){
		features.getUID = this.getUIDXML;
	}

	testRoot.removeChild(testNode);
	testNode = selected = testRoot = null;

	// getAttribute

	features.getAttribute = (features.isHTMLDocument && brokenFormAttributeGetter) ? function(node, name){
		var method = this.attributeGetters[name];
		if (method) return method.call(node);
		var attributeNode = node.getAttributeNode(name);
		return (attributeNode) ? attributeNode.nodeValue : null;
	} : function(node, name){
		var method = this.attributeGetters[name];
		return (method) ? method.call(node) : node.getAttribute(name);
	};

	// hasAttribute

	features.hasAttribute = (root && this.isNativeCode(root.hasAttribute)) ? function(node, attribute){
		return node.hasAttribute(attribute);
	} : function(node, attribute){
		node = node.getAttributeNode(attribute);
		return !!(node && (node.specified || node.nodeValue));
	};

	// contains
	// FIXME: Add specs: local.contains should be different for xml and html documents?
	var nativeRootContains = root && this.isNativeCode(root.contains),
		nativeDocumentContains = document && this.isNativeCode(document.contains);

	features.contains = (nativeRootContains && nativeDocumentContains) ? function(context, node){
		return context.contains(node);
	} : (nativeRootContains && !nativeDocumentContains) ? function(context, node){
		// IE8 does not have .contains on document.
		return context === node || ((context === document) ? document.documentElement : context).contains(node);
	} : (root && root.compareDocumentPosition) ? function(context, node){
		return context === node || !!(context.compareDocumentPosition(node) & 16);
	} : function(context, node){
		if (node) do {
			if (node === context) return true;
		} while ((node = node.parentNode));
		return false;
	};

	// document order sorting
	// credits to Sizzle (http://sizzlejs.com/)

	features.documentSorter = (root.compareDocumentPosition) ? function(a, b){
		if (!a.compareDocumentPosition || !b.compareDocumentPosition) return 0;
		return a.compareDocumentPosition(b) & 4 ? -1 : a === b ? 0 : 1;
	} : ('sourceIndex' in root) ? function(a, b){
		if (!a.sourceIndex || !b.sourceIndex) return 0;
		return a.sourceIndex - b.sourceIndex;
	} : (document.createRange) ? function(a, b){
		if (!a.ownerDocument || !b.ownerDocument) return 0;
		var aRange = a.ownerDocument.createRange(), bRange = b.ownerDocument.createRange();
		aRange.setStart(a, 0);
		aRange.setEnd(a, 0);
		bRange.setStart(b, 0);
		bRange.setEnd(b, 0);
		return aRange.compareBoundaryPoints(Range.START_TO_END, bRange);
	} : null;

	root = null;

	for (feature in features){
		this[feature] = features[feature];
	}
};

// Main Method

var reSimpleSelector = /^([#.]?)((?:[\w-]+|\*))$/,
	reEmptyAttribute = /\[.+[*$^]=(?:""|'')?\]/,
	qsaFailExpCache = {};

local.search = function(context, expression, append, first){

	var found = this.found = (first) ? null : (append || []);

	if (!context) return found;
	else if (context.navigator) context = context.document; // Convert the node from a window to a document
	else if (!context.nodeType) return found;

	// setup

	var parsed, i, node, nodes,
		uniques = this.uniques = {},
		hasOthers = !!(append && append.length),
		contextIsDocument = (context.nodeType == 9);

	if (this.document !== (contextIsDocument ? context : context.ownerDocument)) this.setDocument(context);

	// avoid duplicating items already in the append array
	if (hasOthers) for (i = found.length; i--;) uniques[this.getUID(found[i])] = true;

	// expression checks

	if (typeof expression == 'string'){ // expression is a string

		/*<simple-selectors-override>*/
		var simpleSelector = expression.match(reSimpleSelector);
		simpleSelectors: if (simpleSelector){

			var symbol = simpleSelector[1],
				name = simpleSelector[2];

			if (!symbol){

				if (name == '*' && this.brokenStarGEBTN) break simpleSelectors;
				nodes = context.getElementsByTagName(name);
				if (first) return nodes[0] || null;
				for (i = 0; node = nodes[i++];){
					if (!(hasOthers && uniques[this.getUID(node)])) found.push(node);
				}

			} else if (symbol == '#'){

				if (!this.isHTMLDocument || !contextIsDocument) break simpleSelectors;
				node = context.getElementById(name);
				if (!node) return found;
				if (this.idGetsName && node.getAttributeNode('id').nodeValue != name) break simpleSelectors;
				if (first) return node || null;
				if (!(hasOthers && uniques[this.getUID(node)])) found.push(node);

			} else if (symbol == '.'){

				if (!this.isHTMLDocument || ((!context.getElementsByClassName || this.brokenGEBCN) && context.querySelectorAll)) break simpleSelectors;
				if (context.getElementsByClassName && !this.brokenGEBCN){
					nodes = context.getElementsByClassName(name);
					if (first) return nodes[0] || null;
					for (i = 0; node = nodes[i++];){
						if (!(hasOthers && uniques[this.getUID(node)])) found.push(node);
					}
				} else {
					var matchClass = new RegExp('(^|\\s)'+ Slick.escapeRegExp(name) +'(\\s|$)');
					nodes = context.getElementsByTagName('*');
					for (i = 0; node = nodes[i++];){
						className = node.className;
						if (!(className && matchClass.test(className))) continue;
						if (first) return node;
						if (!(hasOthers && uniques[this.getUID(node)])) found.push(node);
					}
				}

			}

			if (hasOthers) this.sort(found);
			return (first) ? null : found;

		}
		/*</simple-selectors-override>*/

		/*<query-selector-override>*/
		querySelector: if (context.querySelectorAll){

			if (!this.isHTMLDocument
				|| qsaFailExpCache[expression]
				//TODO: only skip when expression is actually mixed case
				|| this.brokenMixedCaseQSA
				|| (this.brokenCheckedQSA && expression.indexOf(':checked') > -1)
				|| (this.brokenEmptyAttributeQSA && reEmptyAttribute.test(expression))
				|| (!contextIsDocument //Abort when !contextIsDocument and...
					//  there are multiple expressions in the selector
					//  since we currently only fix non-document rooted QSA for single expression selectors
					&& expression.indexOf(',') > -1
				)
				|| Slick.disableQSA
			) break querySelector;

			var _expression = expression, _context = context, currentId;
			if (!contextIsDocument){
				// non-document rooted QSA
				// credits to Andrew Dupont
				currentId = _context.getAttribute('id'), slickid = 'slickid__';
				_context.setAttribute('id', slickid);
				_expression = '#' + slickid + ' ' + _expression;
				context = _context.parentNode;
			}

			try {
				if (first) return context.querySelector(_expression) || null;
				else nodes = context.querySelectorAll(_expression);
			} catch (e){
				qsaFailExpCache[expression] = 1;
				break querySelector;
			} finally {
				if (!contextIsDocument){
					if (currentId) _context.setAttribute('id', currentId);
					else _context.removeAttribute('id');
					context = _context;
				}
			}

			if (this.starSelectsClosedQSA) for (i = 0; node = nodes[i++];){
				if (node.nodeName > '@' && !(hasOthers && uniques[this.getUID(node)])) found.push(node);
			} else for (i = 0; node = nodes[i++];){
				if (!(hasOthers && uniques[this.getUID(node)])) found.push(node);
			}

			if (hasOthers) this.sort(found);
			return found;

		}
		/*</query-selector-override>*/

		parsed = this.Slick.parse(expression);
		if (!parsed.length) return found;
	} else if (expression == null){ // there is no expression
		return found;
	} else if (expression.Slick){ // expression is a parsed Slick object
		parsed = expression;
	} else if (this.contains(context.documentElement || context, expression)){ // expression is a node
		(found) ? found.push(expression) : found = expression;
		return found;
	} else { // other junk
		return found;
	}

	/*<pseudo-selectors>*//*<nth-pseudo-selectors>*/

	// cache elements for the nth selectors

	this.posNTH = {};
	this.posNTHLast = {};
	this.posNTHType = {};
	this.posNTHTypeLast = {};

	/*</nth-pseudo-selectors>*//*</pseudo-selectors>*/

	// if append is null and there is only a single selector with one expression use pushArray, else use pushUID
	this.push = (!hasOthers && (first || (parsed.length == 1 && parsed.expressions[0].length == 1))) ? this.pushArray : this.pushUID;

	if (found == null) found = [];

	// default engine

	var j, m, n;
	var combinator, tag, id, classList, classes, attributes, pseudos;
	var currentItems, currentExpression, currentBit, lastBit, expressions = parsed.expressions;

	search: for (i = 0; (currentExpression = expressions[i]); i++) for (j = 0; (currentBit = currentExpression[j]); j++){

		combinator = 'combinator:' + currentBit.combinator;
		if (!this[combinator]) continue search;

		tag        = (this.isXMLDocument) ? currentBit.tag : currentBit.tag.toUpperCase();
		id         = currentBit.id;
		classList  = currentBit.classList;
		classes    = currentBit.classes;
		attributes = currentBit.attributes;
		pseudos    = currentBit.pseudos;
		lastBit    = (j === (currentExpression.length - 1));

		this.bitUniques = {};

		if (lastBit){
			this.uniques = uniques;
			this.found = found;
		} else {
			this.uniques = {};
			this.found = [];
		}

		if (j === 0){
			this[combinator](context, tag, id, classes, attributes, pseudos, classList);
			if (first && lastBit && found.length) break search;
		} else {
			if (first && lastBit) for (m = 0, n = currentItems.length; m < n; m++){
				this[combinator](currentItems[m], tag, id, classes, attributes, pseudos, classList);
				if (found.length) break search;
			} else for (m = 0, n = currentItems.length; m < n; m++) this[combinator](currentItems[m], tag, id, classes, attributes, pseudos, classList);
		}

		currentItems = this.found;
	}

	// should sort if there are nodes in append and if you pass multiple expressions.
	if (hasOthers || (parsed.expressions.length > 1)) this.sort(found);

	return (first) ? (found[0] || null) : found;
};

// Utils

local.uidx = 1;
local.uidk = 'slick-uniqueid';

local.getUIDXML = function(node){
	var uid = node.getAttribute(this.uidk);
	if (!uid){
		uid = this.uidx++;
		node.setAttribute(this.uidk, uid);
	}
	return uid;
};

local.getUIDHTML = function(node){
	return node.uniqueNumber || (node.uniqueNumber = this.uidx++);
};

// sort based on the setDocument documentSorter method.

local.sort = function(results){
	if (!this.documentSorter) return results;
	results.sort(this.documentSorter);
	return results;
};

/*<pseudo-selectors>*//*<nth-pseudo-selectors>*/

local.cacheNTH = {};

local.matchNTH = /^([+-]?\d*)?([a-z]+)?([+-]\d+)?$/;

local.parseNTHArgument = function(argument){
	var parsed = argument.match(this.matchNTH);
	if (!parsed) return false;
	var special = parsed[2] || false;
	var a = parsed[1] || 1;
	if (a == '-') a = -1;
	var b = +parsed[3] || 0;
	parsed =
		(special == 'n')	? {a: a, b: b} :
		(special == 'odd')	? {a: 2, b: 1} :
		(special == 'even')	? {a: 2, b: 0} : {a: 0, b: a};

	return (this.cacheNTH[argument] = parsed);
};

local.createNTHPseudo = function(child, sibling, positions, ofType){
	return function(node, argument){
		var uid = this.getUID(node);
		if (!this[positions][uid]){
			var parent = node.parentNode;
			if (!parent) return false;
			var el = parent[child], count = 1;
			if (ofType){
				var nodeName = node.nodeName;
				do {
					if (el.nodeName != nodeName) continue;
					this[positions][this.getUID(el)] = count++;
				} while ((el = el[sibling]));
			} else {
				do {
					if (el.nodeType != 1) continue;
					this[positions][this.getUID(el)] = count++;
				} while ((el = el[sibling]));
			}
		}
		argument = argument || 'n';
		var parsed = this.cacheNTH[argument] || this.parseNTHArgument(argument);
		if (!parsed) return false;
		var a = parsed.a, b = parsed.b, pos = this[positions][uid];
		if (a == 0) return b == pos;
		if (a > 0){
			if (pos < b) return false;
		} else {
			if (b < pos) return false;
		}
		return ((pos - b) % a) == 0;
	};
};

/*</nth-pseudo-selectors>*//*</pseudo-selectors>*/

local.pushArray = function(node, tag, id, classes, attributes, pseudos){
	if (this.matchSelector(node, tag, id, classes, attributes, pseudos)) this.found.push(node);
};

local.pushUID = function(node, tag, id, classes, attributes, pseudos){
	var uid = this.getUID(node);
	if (!this.uniques[uid] && this.matchSelector(node, tag, id, classes, attributes, pseudos)){
		this.uniques[uid] = true;
		this.found.push(node);
	}
};

local.matchNode = function(node, selector){
	if (this.isHTMLDocument && this.nativeMatchesSelector){
		try {
			return this.nativeMatchesSelector.call(node, selector.replace(/\[([^=]+)=\s*([^'"\]]+?)\s*\]/g, '[$1="$2"]'));
		} catch (matchError){}
	}

	var parsed = this.Slick.parse(selector);
	if (!parsed) return true;

	// simple (single) selectors
	var expressions = parsed.expressions, simpleExpCounter = 0, i, currentExpression;
	for (i = 0; (currentExpression = expressions[i]); i++){
		if (currentExpression.length == 1){
			var exp = currentExpression[0];
			if (this.matchSelector(node, (this.isXMLDocument) ? exp.tag : exp.tag.toUpperCase(), exp.id, exp.classes, exp.attributes, exp.pseudos)) return true;
			simpleExpCounter++;
		}
	}

	if (simpleExpCounter == parsed.length) return false;

	var nodes = this.search(this.document, parsed), item;
	for (i = 0; item = nodes[i++];){
		if (item === node) return true;
	}
	return false;
};

local.matchPseudo = function(node, name, argument){
	var pseudoName = 'pseudo:' + name;
	if (this[pseudoName]) return this[pseudoName](node, argument);
	var attribute = this.getAttribute(node, name);
	return (argument) ? argument == attribute : !!attribute;
};

local.matchSelector = function(node, tag, id, classes, attributes, pseudos){
	if (tag){
		var nodeName = (this.isXMLDocument) ? node.nodeName : node.nodeName.toUpperCase();
		if (tag == '*'){
			if (nodeName < '@') return false; // Fix for comment nodes and closed nodes
		} else {
			if (nodeName != tag) return false;
		}
	}

	if (id && node.getAttribute('id') != id) return false;

	var i, part, cls;
	if (classes) for (i = classes.length; i--;){
		cls = this.getAttribute(node, 'class');
		if (!(cls && classes[i].regexp.test(cls))) return false;
	}
	if (attributes) for (i = attributes.length; i--;){
		part = attributes[i];
		if (part.operator ? !part.test(this.getAttribute(node, part.key)) : !this.hasAttribute(node, part.key)) return false;
	}
	if (pseudos) for (i = pseudos.length; i--;){
		part = pseudos[i];
		if (!this.matchPseudo(node, part.key, part.value)) return false;
	}
	return true;
};

var combinators = {

	' ': function(node, tag, id, classes, attributes, pseudos, classList){ // all child nodes, any level

		var i, item, children;

		if (this.isHTMLDocument){
			getById: if (id){
				item = this.document.getElementById(id);
				if ((!item && node.all) || (this.idGetsName && item && item.getAttributeNode('id').nodeValue != id)){
					// all[id] returns all the elements with that name or id inside node
					// if theres just one it will return the element, else it will be a collection
					children = node.all[id];
					if (!children) return;
					if (!children[0]) children = [children];
					for (i = 0; item = children[i++];){
						var idNode = item.getAttributeNode('id');
						if (idNode && idNode.nodeValue == id){
							this.push(item, tag, null, classes, attributes, pseudos);
							break;
						}
					}
					return;
				}
				if (!item){
					// if the context is in the dom we return, else we will try GEBTN, breaking the getById label
					if (this.contains(this.root, node)) return;
					else break getById;
				} else if (this.document !== node && !this.contains(node, item)) return;
				this.push(item, tag, null, classes, attributes, pseudos);
				return;
			}
			getByClass: if (classes && node.getElementsByClassName && !this.brokenGEBCN){
				children = node.getElementsByClassName(classList.join(' '));
				if (!(children && children.length)) break getByClass;
				for (i = 0; item = children[i++];) this.push(item, tag, id, null, attributes, pseudos);
				return;
			}
		}
		getByTag: {
			children = node.getElementsByTagName(tag);
			if (!(children && children.length)) break getByTag;
			if (!this.brokenStarGEBTN) tag = null;
			for (i = 0; item = children[i++];) this.push(item, tag, id, classes, attributes, pseudos);
		}
	},

	'>': function(node, tag, id, classes, attributes, pseudos){ // direct children
		if ((node = node.firstChild)) do {
			if (node.nodeType == 1) this.push(node, tag, id, classes, attributes, pseudos);
		} while ((node = node.nextSibling));
	},

	'+': function(node, tag, id, classes, attributes, pseudos){ // next sibling
		while ((node = node.nextSibling)) if (node.nodeType == 1){
			this.push(node, tag, id, classes, attributes, pseudos);
			break;
		}
	},

	'^': function(node, tag, id, classes, attributes, pseudos){ // first child
		node = node.firstChild;
		if (node){
			if (node.nodeType == 1) this.push(node, tag, id, classes, attributes, pseudos);
			else this['combinator:+'](node, tag, id, classes, attributes, pseudos);
		}
	},

	'~': function(node, tag, id, classes, attributes, pseudos){ // next siblings
		while ((node = node.nextSibling)){
			if (node.nodeType != 1) continue;
			var uid = this.getUID(node);
			if (this.bitUniques[uid]) break;
			this.bitUniques[uid] = true;
			this.push(node, tag, id, classes, attributes, pseudos);
		}
	},

	'++': function(node, tag, id, classes, attributes, pseudos){ // next sibling and previous sibling
		this['combinator:+'](node, tag, id, classes, attributes, pseudos);
		this['combinator:!+'](node, tag, id, classes, attributes, pseudos);
	},

	'~~': function(node, tag, id, classes, attributes, pseudos){ // next siblings and previous siblings
		this['combinator:~'](node, tag, id, classes, attributes, pseudos);
		this['combinator:!~'](node, tag, id, classes, attributes, pseudos);
	},

	'!': function(node, tag, id, classes, attributes, pseudos){ // all parent nodes up to document
		while ((node = node.parentNode)) if (node !== this.document) this.push(node, tag, id, classes, attributes, pseudos);
	},

	'!>': function(node, tag, id, classes, attributes, pseudos){ // direct parent (one level)
		node = node.parentNode;
		if (node !== this.document) this.push(node, tag, id, classes, attributes, pseudos);
	},

	'!+': function(node, tag, id, classes, attributes, pseudos){ // previous sibling
		while ((node = node.previousSibling)) if (node.nodeType == 1){
			this.push(node, tag, id, classes, attributes, pseudos);
			break;
		}
	},

	'!^': function(node, tag, id, classes, attributes, pseudos){ // last child
		node = node.lastChild;
		if (node){
			if (node.nodeType == 1) this.push(node, tag, id, classes, attributes, pseudos);
			else this['combinator:!+'](node, tag, id, classes, attributes, pseudos);
		}
	},

	'!~': function(node, tag, id, classes, attributes, pseudos){ // previous siblings
		while ((node = node.previousSibling)){
			if (node.nodeType != 1) continue;
			var uid = this.getUID(node);
			if (this.bitUniques[uid]) break;
			this.bitUniques[uid] = true;
			this.push(node, tag, id, classes, attributes, pseudos);
		}
	}

};

for (var c in combinators) local['combinator:' + c] = combinators[c];

var pseudos = {

	/*<pseudo-selectors>*/

	'empty': function(node){
		var child = node.firstChild;
		return !(child && child.nodeType == 1) && !(node.innerText || node.textContent || '').length;
	},

	'not': function(node, expression){
		return !this.matchNode(node, expression);
	},

	'contains': function(node, text){
		return (node.innerText || node.textContent || '').indexOf(text) > -1;
	},

	'first-child': function(node){
		while ((node = node.previousSibling)) if (node.nodeType == 1) return false;
		return true;
	},

	'last-child': function(node){
		while ((node = node.nextSibling)) if (node.nodeType == 1) return false;
		return true;
	},

	'only-child': function(node){
		var prev = node;
		while ((prev = prev.previousSibling)) if (prev.nodeType == 1) return false;
		var next = node;
		while ((next = next.nextSibling)) if (next.nodeType == 1) return false;
		return true;
	},

	/*<nth-pseudo-selectors>*/

	'nth-child': local.createNTHPseudo('firstChild', 'nextSibling', 'posNTH'),

	'nth-last-child': local.createNTHPseudo('lastChild', 'previousSibling', 'posNTHLast'),

	'nth-of-type': local.createNTHPseudo('firstChild', 'nextSibling', 'posNTHType', true),

	'nth-last-of-type': local.createNTHPseudo('lastChild', 'previousSibling', 'posNTHTypeLast', true),

	'index': function(node, index){
		return this['pseudo:nth-child'](node, '' + (index + 1));
	},

	'even': function(node){
		return this['pseudo:nth-child'](node, '2n');
	},

	'odd': function(node){
		return this['pseudo:nth-child'](node, '2n+1');
	},

	/*</nth-pseudo-selectors>*/

	/*<of-type-pseudo-selectors>*/

	'first-of-type': function(node){
		var nodeName = node.nodeName;
		while ((node = node.previousSibling)) if (node.nodeName == nodeName) return false;
		return true;
	},

	'last-of-type': function(node){
		var nodeName = node.nodeName;
		while ((node = node.nextSibling)) if (node.nodeName == nodeName) return false;
		return true;
	},

	'only-of-type': function(node){
		var prev = node, nodeName = node.nodeName;
		while ((prev = prev.previousSibling)) if (prev.nodeName == nodeName) return false;
		var next = node;
		while ((next = next.nextSibling)) if (next.nodeName == nodeName) return false;
		return true;
	},

	/*</of-type-pseudo-selectors>*/

	// custom pseudos

	'enabled': function(node){
		return !node.disabled;
	},

	'disabled': function(node){
		return node.disabled;
	},

	'checked': function(node){
		return node.checked || node.selected;
	},

	'focus': function(node){
		return this.isHTMLDocument && this.document.activeElement === node && (node.href || node.type || this.hasAttribute(node, 'tabindex'));
	},

	'root': function(node){
		return (node === this.root);
	},

	'selected': function(node){
		return node.selected;
	}

	/*</pseudo-selectors>*/
};

for (var p in pseudos) local['pseudo:' + p] = pseudos[p];

// attributes methods

var attributeGetters = local.attributeGetters = {

	'for': function(){
		return ('htmlFor' in this) ? this.htmlFor : this.getAttribute('for');
	},

	'href': function(){
		return ('href' in this) ? this.getAttribute('href', 2) : this.getAttribute('href');
	},

	'style': function(){
		return (this.style) ? this.style.cssText : this.getAttribute('style');
	},

	'tabindex': function(){
		var attributeNode = this.getAttributeNode('tabindex');
		return (attributeNode && attributeNode.specified) ? attributeNode.nodeValue : null;
	},

	'type': function(){
		return this.getAttribute('type');
	},

	'maxlength': function(){
		var attributeNode = this.getAttributeNode('maxLength');
		return (attributeNode && attributeNode.specified) ? attributeNode.nodeValue : null;
	}

};

attributeGetters.MAXLENGTH = attributeGetters.maxLength = attributeGetters.maxlength;

// Slick

var Slick = local.Slick = (this.Slick || {});

Slick.version = '1.1.7';

// Slick finder

Slick.search = function(context, expression, append){
	return local.search(context, expression, append);
};

Slick.find = function(context, expression){
	return local.search(context, expression, null, true);
};

// Slick containment checker

Slick.contains = function(container, node){
	local.setDocument(container);
	return local.contains(container, node);
};

// Slick attribute getter

Slick.getAttribute = function(node, name){
	local.setDocument(node);
	return local.getAttribute(node, name);
};

Slick.hasAttribute = function(node, name){
	local.setDocument(node);
	return local.hasAttribute(node, name);
};

// Slick matcher

Slick.match = function(node, selector){
	if (!(node && selector)) return false;
	if (!selector || selector === node) return true;
	local.setDocument(node);
	return local.matchNode(node, selector);
};

// Slick attribute accessor

Slick.defineAttributeGetter = function(name, fn){
	local.attributeGetters[name] = fn;
	return this;
};

Slick.lookupAttributeGetter = function(name){
	return local.attributeGetters[name];
};

// Slick pseudo accessor

Slick.definePseudo = function(name, fn){
	local['pseudo:' + name] = function(node, argument){
		return fn.call(node, argument);
	};
	return this;
};

Slick.lookupPseudo = function(name){
	var pseudo = local['pseudo:' + name];
	if (pseudo) return function(argument){
		return pseudo.call(this, argument);
	};
	return null;
};

// Slick overrides accessor

Slick.override = function(regexp, fn){
	local.override(regexp, fn);
	return this;
};

Slick.isXML = local.isXML;

Slick.uidOf = function(node){
	return local.getUIDHTML(node);
};

if (!this.Slick) this.Slick = Slick;

}).apply(/*<CommonJS>*/(typeof exports != 'undefined') ? exports : /*</CommonJS>*/this);

/*
---

name: Element

description: One of the most important items in MooTools. Contains the dollar function, the dollars function, and an handful of cross-browser, time-saver methods to let you easily work with HTML Elements.

license: MIT-style license.

requires: [Window, Document, Array, String, Function, Object, Number, Slick.Parser, Slick.Finder]

provides: [Element, Elements, $, $$, IFrame, Selectors]

...
*/

var Element = this.Element = function(tag, props){
	var konstructor = Element.Constructors[tag];
	if (konstructor) return konstructor(props);
	if (typeof tag != 'string') return document.id(tag).set(props);

	if (!props) props = {};

	if (!(/^[\w-]+$/).test(tag)){
		var parsed = Slick.parse(tag).expressions[0][0];
		tag = (parsed.tag == '*') ? 'div' : parsed.tag;
		if (parsed.id && props.id == null) props.id = parsed.id;

		var attributes = parsed.attributes;
		if (attributes) for (var attr, i = 0, l = attributes.length; i < l; i++){
			attr = attributes[i];
			if (props[attr.key] != null) continue;

			if (attr.value != null && attr.operator == '=') props[attr.key] = attr.value;
			else if (!attr.value && !attr.operator) props[attr.key] = true;
		}

		if (parsed.classList && props['class'] == null) props['class'] = parsed.classList.join(' ');
	}

	return document.newElement(tag, props);
};


if (Browser.Element){
	Element.prototype = Browser.Element.prototype;
	// IE8 and IE9 require the wrapping.
	Element.prototype._fireEvent = (function(fireEvent){
		return function(type, event){
			return fireEvent.call(this, type, event);
		};
	})(Element.prototype.fireEvent);
}

new Type('Element', Element).mirror(function(name){
	if (Array.prototype[name]) return;

	var obj = {};
	obj[name] = function(){
		var results = [], args = arguments, elements = true;
		for (var i = 0, l = this.length; i < l; i++){
			var element = this[i], result = results[i] = element[name].apply(element, args);
			elements = (elements && typeOf(result) == 'element');
		}
		return (elements) ? new Elements(results) : results;
	};

	Elements.implement(obj);
});

if (!Browser.Element){
	Element.parent = Object;

	Element.Prototype = {
		'$constructor': Element,
		'$family': Function.convert('element').hide()
	};

	Element.mirror(function(name, method){
		Element.Prototype[name] = method;
	});
}

Element.Constructors = {};



var IFrame = new Type('IFrame', function(){
	var params = Array.link(arguments, {
		properties: Type.isObject,
		iframe: function(obj){
			return (obj != null);
		}
	});

	var props = params.properties || {}, iframe;
	if (params.iframe) iframe = document.id(params.iframe);
	var onload = props.onload || function(){};
	delete props.onload;
	props.id = props.name = [props.id, props.name, iframe ? (iframe.id || iframe.name) : 'IFrame_' + String.uniqueID()].pick();
	iframe = new Element(iframe || 'iframe', props);

	var onLoad = function(){
		onload.call(iframe.contentWindow);
	};

	if (window.frames[props.id]) onLoad();
	else iframe.addListener('load', onLoad);
	return iframe;
});

var Elements = this.Elements = function(nodes){
	if (nodes && nodes.length){
		var uniques = {}, node;
		for (var i = 0; node = nodes[i++];){
			var uid = Slick.uidOf(node);
			if (!uniques[uid]){
				uniques[uid] = true;
				this.push(node);
			}
		}
	}
};

Elements.prototype = {length: 0};
Elements.parent = Array;

new Type('Elements', Elements).implement({

	filter: function(filter, bind){
		if (!filter) return this;
		return new Elements(Array.filter(this, (typeOf(filter) == 'string') ? function(item){
			return item.match(filter);
		} : filter, bind));
	}.protect(),

	push: function(){
		var length = this.length;
		for (var i = 0, l = arguments.length; i < l; i++){
			var item = document.id(arguments[i]);
			if (item) this[length++] = item;
		}
		return (this.length = length);
	}.protect(),

	unshift: function(){
		var items = [];
		for (var i = 0, l = arguments.length; i < l; i++){
			var item = document.id(arguments[i]);
			if (item) items.push(item);
		}
		return Array.prototype.unshift.apply(this, items);
	}.protect(),

	concat: function(){
		var newElements = new Elements(this);
		for (var i = 0, l = arguments.length; i < l; i++){
			var item = arguments[i];
			if (Type.isEnumerable(item)) newElements.append(item);
			else newElements.push(item);
		}
		return newElements;
	}.protect(),

	append: function(collection){
		for (var i = 0, l = collection.length; i < l; i++) this.push(collection[i]);
		return this;
	}.protect(),

	empty: function(){
		while (this.length) delete this[--this.length];
		return this;
	}.protect()

});



(function(){

// FF, IE
var splice = Array.prototype.splice, object = {'0': 0, '1': 1, length: 2};

splice.call(object, 1, 1);
if (object[1] == 1) Elements.implement('splice', function(){
	var length = this.length;
	var result = splice.apply(this, arguments);
	while (length >= this.length) delete this[length--];
	return result;
}.protect());

Array.forEachMethod(function(method, name){
	Elements.implement(name, method);
});

Array.mirror(Elements);

/*<ltIE8>*/
var createElementAcceptsHTML;
try {
	createElementAcceptsHTML = (document.createElement('<input name=x>').name == 'x');
} catch (e){}

var escapeQuotes = function(html){
	return ('' + html).replace(/&/g, '&amp;').replace(/"/g, '&quot;');
};
/*</ltIE8>*/

/*<ltIE9>*/
// #2479 - IE8 Cannot set HTML of style element
var canChangeStyleHTML = (function(){
	var div = document.createElement('style'),
		flag = false;
	try {
		div.innerHTML = '#justTesing{margin: 0px;}';
		flag = !!div.innerHTML;
	} catch (e){}
	return flag;
})();
/*</ltIE9>*/

Document.implement({

	newElement: function(tag, props){
		if (props){
			if (props.checked != null) props.defaultChecked = props.checked;
			if ((props.type == 'checkbox' || props.type == 'radio') && props.value == null) props.value = 'on';
			/*<ltIE9>*/ // IE needs the type to be set before changing content of style element
			if (!canChangeStyleHTML && tag == 'style'){
				var styleElement = document.createElement('style');
				styleElement.setAttribute('type', 'text/css');
				if (props.type) delete props.type;
				return this.id(styleElement).set(props);
			}
			/*</ltIE9>*/
			/*<ltIE8>*/// Fix for readonly name and type properties in IE < 8
			if (createElementAcceptsHTML){
				tag = '<' + tag;
				if (props.name) tag += ' name="' + escapeQuotes(props.name) + '"';
				if (props.type) tag += ' type="' + escapeQuotes(props.type) + '"';
				tag += '>';
				delete props.name;
				delete props.type;
			}
			/*</ltIE8>*/
		}
		return this.id(this.createElement(tag)).set(props);
	}

});

})();

(function(){

Slick.uidOf(window);
Slick.uidOf(document);

Document.implement({

	newTextNode: function(text){
		return this.createTextNode(text);
	},

	getDocument: function(){
		return this;
	},

	getWindow: function(){
		return this.window;
	},

	id: (function(){

		var types = {

			string: function(id, nocash, doc){
				id = Slick.find(doc, '#' + id.replace(/(\W)/g, '\\$1'));
				return (id) ? types.element(id, nocash) : null;
			},

			element: function(el, nocash){
				Slick.uidOf(el);
				if (!nocash && !el.$family && !(/^(?:object|embed)$/i).test(el.tagName)){
					var fireEvent = el.fireEvent;
					// wrapping needed in IE7, or else crash
					el._fireEvent = function(type, event){
						return fireEvent(type, event);
					};
					Object.append(el, Element.Prototype);
				}
				return el;
			},

			object: function(obj, nocash, doc){
				if (obj.toElement) return types.element(obj.toElement(doc), nocash);
				return null;
			}

		};

		types.textnode = types.whitespace = types.window = types.document = function(zero){
			return zero;
		};

		return function(el, nocash, doc){
			if (el && el.$family && el.uniqueNumber) return el;
			var type = typeOf(el);
			return (types[type]) ? types[type](el, nocash, doc || document) : null;
		};

	})()

});

if (window.$ == null) Window.implement('$', function(el, nc){
	return document.id(el, nc, this.document);
});

Window.implement({

	getDocument: function(){
		return this.document;
	},

	getWindow: function(){
		return this;
	}

});

[Document, Element].invoke('implement', {

	getElements: function(expression){
		return Slick.search(this, expression, new Elements);
	},

	getElement: function(expression){
		return document.id(Slick.find(this, expression));
	}

});

var contains = {contains: function(element){
	return Slick.contains(this, element);
}};

if (!document.contains) Document.implement(contains);
if (!document.createElement('div').contains) Element.implement(contains);



// tree walking

var injectCombinator = function(expression, combinator){
	if (!expression) return combinator;

	expression = Object.clone(Slick.parse(expression));

	var expressions = expression.expressions;
	for (var i = expressions.length; i--;)
		expressions[i][0].combinator = combinator;

	return expression;
};

Object.forEach({
	getNext: '~',
	getPrevious: '!~',
	getParent: '!'
}, function(combinator, method){
	Element.implement(method, function(expression){
		return this.getElement(injectCombinator(expression, combinator));
	});
});

Object.forEach({
	getAllNext: '~',
	getAllPrevious: '!~',
	getSiblings: '~~',
	getChildren: '>',
	getParents: '!'
}, function(combinator, method){
	Element.implement(method, function(expression){
		return this.getElements(injectCombinator(expression, combinator));
	});
});

Element.implement({

	getFirst: function(expression){
		return document.id(Slick.search(this, injectCombinator(expression, '>'))[0]);
	},

	getLast: function(expression){
		return document.id(Slick.search(this, injectCombinator(expression, '>')).getLast());
	},

	getWindow: function(){
		return this.ownerDocument.window;
	},

	getDocument: function(){
		return this.ownerDocument;
	},

	getElementById: function(id){
		return document.id(Slick.find(this, '#' + ('' + id).replace(/(\W)/g, '\\$1')));
	},

	match: function(expression){
		return !expression || Slick.match(this, expression);
	}

});



if (window.$$ == null) Window.implement('$$', function(selector){
	if (arguments.length == 1){
		if (typeof selector == 'string') return Slick.search(this.document, selector, new Elements);
		else if (Type.isEnumerable(selector)) return new Elements(selector);
	}
	return new Elements(arguments);
});

// Inserters

var inserters = {

	before: function(context, element){
		var parent = element.parentNode;
		if (parent) parent.insertBefore(context, element);
	},

	after: function(context, element){
		var parent = element.parentNode;
		if (parent) parent.insertBefore(context, element.nextSibling);
	},

	bottom: function(context, element){
		element.appendChild(context);
	},

	top: function(context, element){
		element.insertBefore(context, element.firstChild);
	}

};

inserters.inside = inserters.bottom;



// getProperty / setProperty

var propertyGetters = {}, propertySetters = {};

// properties

var properties = {};
Array.forEach([
	'type', 'value', 'defaultValue', 'accessKey', 'cellPadding', 'cellSpacing', 'colSpan',
	'frameBorder', 'rowSpan', 'tabIndex', 'useMap'
], function(property){
	properties[property.toLowerCase()] = property;
});

properties.html = 'innerHTML';
properties.text = (document.createElement('div').textContent == null) ? 'innerText': 'textContent';

Object.forEach(properties, function(real, key){
	propertySetters[key] = function(node, value){
		node[real] = value;
	};
	propertyGetters[key] = function(node){
		return node[real];
	};
});

/*<ltIE9>*/
propertySetters.text = (function(){
	return function(node, value){
		if (node.get('tag') == 'style') node.set('html', value);
		else node[properties.text] = value;
	};
})(propertySetters.text);

propertyGetters.text = (function(getter){
	return function(node){
		return (node.get('tag') == 'style') ? node.innerHTML : getter(node);
	};
})(propertyGetters.text);
/*</ltIE9>*/

// Booleans

var bools = [
	'compact', 'nowrap', 'ismap', 'declare', 'noshade', 'checked',
	'disabled', 'readOnly', 'multiple', 'selected', 'noresize',
	'defer', 'defaultChecked', 'autofocus', 'controls', 'autoplay',
	'loop'
];

var booleans = {};
Array.forEach(bools, function(bool){
	var lower = bool.toLowerCase();
	booleans[lower] = bool;
	propertySetters[lower] = function(node, value){
		node[bool] = !!value;
	};
	propertyGetters[lower] = function(node){
		return !!node[bool];
	};
});

// Special cases

Object.append(propertySetters, {

	'class': function(node, value){
		('className' in node) ? node.className = (value || '') : node.setAttribute('class', value);
	},

	'for': function(node, value){
		('htmlFor' in node) ? node.htmlFor = value : node.setAttribute('for', value);
	},

	'style': function(node, value){
		(node.style) ? node.style.cssText = value : node.setAttribute('style', value);
	},

	'value': function(node, value){
		node.value = (value != null) ? value : '';
	}

});

propertyGetters['class'] = function(node){
	return ('className' in node) ? node.className || null : node.getAttribute('class');
};

/* <webkit> */
var el = document.createElement('button');
// IE sets type as readonly and throws
try { el.type = 'button'; } catch (e){}
if (el.type != 'button') propertySetters.type = function(node, value){
	node.setAttribute('type', value);
};
el = null;
/* </webkit> */

/*<IE>*/

/*<ltIE9>*/
// #2479 - IE8 Cannot set HTML of style element
var canChangeStyleHTML = (function(){
	var div = document.createElement('style'),
		flag = false;
	try {
		div.innerHTML = '#justTesing{margin: 0px;}';
		flag = !!div.innerHTML;
	} catch (e){}
	return flag;
})();
/*</ltIE9>*/

var input = document.createElement('input'), volatileInputValue, html5InputSupport;

// #2178
input.value = 't';
input.type = 'submit';
volatileInputValue = input.value != 't';

// #2443 - IE throws "Invalid Argument" when trying to use html5 input types
try {
	input.value = '';
	input.type = 'email';
	html5InputSupport = input.type == 'email';
} catch (e){}

input = null;

if (volatileInputValue || !html5InputSupport) propertySetters.type = function(node, type){
	try {
		var value = node.value;
		node.type = type;
		node.value = value;
	} catch (e){}
};
/*</IE>*/

/* getProperty, setProperty */

/* <ltIE9> */
var pollutesGetAttribute = (function(div){
	div.random = 'attribute';
	return (div.getAttribute('random') == 'attribute');
})(document.createElement('div'));

var hasCloneBug = (function(test){
	test.innerHTML = '<object><param name="should_fix" value="the unknown" /></object>';
	return test.cloneNode(true).firstChild.childNodes.length != 1;
})(document.createElement('div'));
/* </ltIE9> */

var hasClassList = !!document.createElement('div').classList;

var classes = function(className){
	var classNames = (className || '').clean().split(' '), uniques = {};
	return classNames.filter(function(className){
		if (className !== '' && !uniques[className]) return uniques[className] = className;
	});
};

var addToClassList = function(name){
	this.classList.add(name);
};

var removeFromClassList = function(name){
	this.classList.remove(name);
};

Element.implement({

	setProperty: function(name, value){
		var setter = propertySetters[name.toLowerCase()];
		if (setter){
			setter(this, value);
		} else {
			/* <ltIE9> */
			var attributeWhiteList;
			if (pollutesGetAttribute) attributeWhiteList = this.retrieve('$attributeWhiteList', {});
			/* </ltIE9> */

			if (value == null){
				this.removeAttribute(name);
				/* <ltIE9> */
				if (pollutesGetAttribute) delete attributeWhiteList[name];
				/* </ltIE9> */
			} else {
				this.setAttribute(name, '' + value);
				/* <ltIE9> */
				if (pollutesGetAttribute) attributeWhiteList[name] = true;
				/* </ltIE9> */
			}
		}
		return this;
	},

	setProperties: function(attributes){
		for (var attribute in attributes) this.setProperty(attribute, attributes[attribute]);
		return this;
	},

	getProperty: function(name){
		var getter = propertyGetters[name.toLowerCase()];
		if (getter) return getter(this);
		/* <ltIE9> */
		if (pollutesGetAttribute){
			var attr = this.getAttributeNode(name), attributeWhiteList = this.retrieve('$attributeWhiteList', {});
			if (!attr) return null;
			if (attr.expando && !attributeWhiteList[name]){
				var outer = this.outerHTML;
				// segment by the opening tag and find mention of attribute name
				if (outer.substr(0, outer.search(/\/?['"]?>(?![^<]*<['"])/)).indexOf(name) < 0) return null;
				attributeWhiteList[name] = true;
			}
		}
		/* </ltIE9> */
		var result = Slick.getAttribute(this, name);
		return (!result && !Slick.hasAttribute(this, name)) ? null : result;
	},

	getProperties: function(){
		var args = Array.convert(arguments);
		return args.map(this.getProperty, this).associate(args);
	},

	removeProperty: function(name){
		return this.setProperty(name, null);
	},

	removeProperties: function(){
		Array.each(arguments, this.removeProperty, this);
		return this;
	},

	set: function(prop, value){
		var property = Element.Properties[prop];
		(property && property.set) ? property.set.call(this, value) : this.setProperty(prop, value);
	}.overloadSetter(),

	get: function(prop){
		var property = Element.Properties[prop];
		return (property && property.get) ? property.get.apply(this) : this.getProperty(prop);
	}.overloadGetter(),

	erase: function(prop){
		var property = Element.Properties[prop];
		(property && property.erase) ? property.erase.apply(this) : this.removeProperty(prop);
		return this;
	},

	hasClass: hasClassList ? function(className){
		return this.classList.contains(className);
	} : function(className){
		return classes(this.className).contains(className);
	},

	addClass: hasClassList ? function(className){
		classes(className).forEach(addToClassList, this);
		return this;
	} : function(className){
		this.className = classes(className + ' ' + this.className).join(' ');
		return this;
	},

	removeClass: hasClassList ? function(className){
		classes(className).forEach(removeFromClassList, this);
		return this;
	} : function(className){
		var classNames = classes(this.className);
		classes(className).forEach(classNames.erase, classNames);
		this.className = classNames.join(' ');
		return this;
	},

	toggleClass: function(className, force){
		if (force == null) force = !this.hasClass(className);
		return (force) ? this.addClass(className) : this.removeClass(className);
	},

	adopt: function(){
		var parent = this, fragment, elements = Array.flatten(arguments), length = elements.length;
		if (length > 1) parent = fragment = document.createDocumentFragment();

		for (var i = 0; i < length; i++){
			var element = document.id(elements[i], true);
			if (element) parent.appendChild(element);
		}

		if (fragment) this.appendChild(fragment);

		return this;
	},

	appendText: function(text, where){
		return this.grab(this.getDocument().newTextNode(text), where);
	},

	grab: function(el, where){
		inserters[where || 'bottom'](document.id(el, true), this);
		return this;
	},

	inject: function(el, where){
		inserters[where || 'bottom'](this, document.id(el, true));
		return this;
	},

	replaces: function(el){
		el = document.id(el, true);
		el.parentNode.replaceChild(this, el);
		return this;
	},

	wraps: function(el, where){
		el = document.id(el, true);
		return this.replaces(el).grab(el, where);
	},

	getSelected: function(){
		this.selectedIndex; // Safari 3.2.1
		return new Elements(Array.convert(this.options).filter(function(option){
			return option.selected;
		}));
	},

	toQueryString: function(){
		var queryString = [];
		this.getElements('input, select, textarea').each(function(el){
			var type = el.type;
			if (!el.name || el.disabled || type == 'submit' || type == 'reset' || type == 'file' || type == 'image') return;

			var value = (el.get('tag') == 'select') ? el.getSelected().map(function(opt){
				// IE
				return document.id(opt).get('value');
			}) : ((type == 'radio' || type == 'checkbox') && !el.checked) ? null : el.get('value');

			Array.convert(value).each(function(val){
				if (typeof val != 'undefined') queryString.push(encodeURIComponent(el.name) + '=' + encodeURIComponent(val));
			});
		});
		return queryString.join('&');
	}

});


// appendHTML

var appendInserters = {
	before: 'beforeBegin',
	after: 'afterEnd',
	bottom: 'beforeEnd',
	top: 'afterBegin',
	inside: 'beforeEnd'
};

Element.implement('appendHTML', ('insertAdjacentHTML' in document.createElement('div')) ? function(html, where){
	this.insertAdjacentHTML(appendInserters[where || 'bottom'], html);
	return this;
} : function(html, where){
	var temp = new Element('div', {html: html}),
		children = temp.childNodes,
		fragment = temp.firstChild;

	if (!fragment) return this;
	if (children.length > 1){
		fragment = document.createDocumentFragment();
		for (var i = 0, l = children.length; i < l; i++){
			fragment.appendChild(children[i]);
		}
	}

	inserters[where || 'bottom'](fragment, this);
	return this;
});

var collected = {}, storage = {};

var get = function(uid){
	return (storage[uid] || (storage[uid] = {}));
};

var clean = function(item){
	var uid = item.uniqueNumber;
	if (item.removeEvents) item.removeEvents();
	if (item.clearAttributes) item.clearAttributes();
	if (uid != null){
		delete collected[uid];
		delete storage[uid];
	}
	return item;
};

var formProps = {input: 'checked', option: 'selected', textarea: 'value'};

Element.implement({

	destroy: function(){
		var children = clean(this).getElementsByTagName('*');
		Array.each(children, clean);
		Element.dispose(this);
		return null;
	},

	empty: function(){
		Array.convert(this.childNodes).each(Element.dispose);
		return this;
	},

	dispose: function(){
		return (this.parentNode) ? this.parentNode.removeChild(this) : this;
	},

	clone: function(contents, keepid){
		contents = contents !== false;
		var clone = this.cloneNode(contents), ce = [clone], te = [this], i;

		if (contents){
			ce.append(Array.convert(clone.getElementsByTagName('*')));
			te.append(Array.convert(this.getElementsByTagName('*')));
		}

		for (i = ce.length; i--;){
			var node = ce[i], element = te[i];
			if (!keepid) node.removeAttribute('id');
			/*<ltIE9>*/
			if (node.clearAttributes){
				node.clearAttributes();
				node.mergeAttributes(element);
				node.removeAttribute('uniqueNumber');
				if (node.options){
					var no = node.options, eo = element.options;
					for (var j = no.length; j--;) no[j].selected = eo[j].selected;
				}
			}
			/*</ltIE9>*/
			var prop = formProps[element.tagName.toLowerCase()];
			if (prop && element[prop]) node[prop] = element[prop];
		}

		/*<ltIE9>*/
		if (hasCloneBug){
			var co = clone.getElementsByTagName('object'), to = this.getElementsByTagName('object');
			for (i = co.length; i--;) co[i].outerHTML = to[i].outerHTML;
		}
		/*</ltIE9>*/
		return document.id(clone);
	}

});

[Element, Window, Document].invoke('implement', {

	addListener: function(type, fn){
		if (window.attachEvent && !window.addEventListener){
			collected[Slick.uidOf(this)] = this;
		}
		if (this.addEventListener) this.addEventListener(type, fn, !!arguments[2]);
		else this.attachEvent('on' + type, fn);
		return this;
	},

	removeListener: function(type, fn){
		if (this.removeEventListener) this.removeEventListener(type, fn, !!arguments[2]);
		else this.detachEvent('on' + type, fn);
		return this;
	},

	retrieve: function(property, dflt){
		var storage = get(Slick.uidOf(this)), prop = storage[property];
		if (dflt != null && prop == null) prop = storage[property] = dflt;
		return prop != null ? prop : null;
	},

	store: function(property, value){
		var storage = get(Slick.uidOf(this));
		storage[property] = value;
		return this;
	},

	eliminate: function(property){
		var storage = get(Slick.uidOf(this));
		delete storage[property];
		return this;
	}

});

/*<ltIE9>*/
if (window.attachEvent && !window.addEventListener){
	var gc = function(){
		Object.each(collected, clean);
		if (window.CollectGarbage) CollectGarbage();
		window.removeListener('unload', gc);
	};
	window.addListener('unload', gc);
}
/*</ltIE9>*/

Element.Properties = {};



Element.Properties.style = {

	set: function(style){
		this.style.cssText = style;
	},

	get: function(){
		return this.style.cssText;
	},

	erase: function(){
		this.style.cssText = '';
	}

};

Element.Properties.tag = {

	get: function(){
		return this.tagName.toLowerCase();
	}

};

Element.Properties.html = {

	set: function(html){
		if (html == null) html = '';
		else if (typeOf(html) == 'array') html = html.join('');

		/*<ltIE9>*/
		if (this.styleSheet && !canChangeStyleHTML) this.styleSheet.cssText = html;
		else /*</ltIE9>*/this.innerHTML = html;
	},
	erase: function(){
		this.set('html', '');
	}

};

var supportsHTML5Elements = true, supportsTableInnerHTML = true, supportsTRInnerHTML = true;

/*<ltIE9>*/
// technique by jdbarlett - http://jdbartlett.com/innershiv/
var div = document.createElement('div');
var fragment;
div.innerHTML = '<nav></nav>';
supportsHTML5Elements = (div.childNodes.length == 1);
if (!supportsHTML5Elements){
	var tags = 'abbr article aside audio canvas datalist details figcaption figure footer header hgroup mark meter nav output progress section summary time video'.split(' ');
	fragment = document.createDocumentFragment(), l = tags.length;
	while (l--) fragment.createElement(tags[l]);
}
div = null;
/*</ltIE9>*/

/*<IE>*/
supportsTableInnerHTML = Function.attempt(function(){
	var table = document.createElement('table');
	table.innerHTML = '<tr><td></td></tr>';
	return true;
});

/*<ltFF4>*/
var tr = document.createElement('tr'), html = '<td></td>';
tr.innerHTML = html;
supportsTRInnerHTML = (tr.innerHTML == html);
tr = null;
/*</ltFF4>*/

if (!supportsTableInnerHTML || !supportsTRInnerHTML || !supportsHTML5Elements){

	Element.Properties.html.set = (function(set){

		var translations = {
			table: [1, '<table>', '</table>'],
			select: [1, '<select>', '</select>'],
			tbody: [2, '<table><tbody>', '</tbody></table>'],
			tr: [3, '<table><tbody><tr>', '</tr></tbody></table>']
		};

		translations.thead = translations.tfoot = translations.tbody;

		return function(html){

			/*<ltIE9>*/
			if (this.styleSheet) return set.call(this, html);
			/*</ltIE9>*/
			var wrap = translations[this.get('tag')];
			if (!wrap && !supportsHTML5Elements) wrap = [0, '', ''];
			if (!wrap) return set.call(this, html);

			var level = wrap[0], wrapper = document.createElement('div'), target = wrapper;
			if (!supportsHTML5Elements) fragment.appendChild(wrapper);
			wrapper.innerHTML = [wrap[1], html, wrap[2]].flatten().join('');
			while (level--) target = target.firstChild;
			this.empty().adopt(target.childNodes);
			if (!supportsHTML5Elements) fragment.removeChild(wrapper);
			wrapper = null;
		};

	})(Element.Properties.html.set);
}
/*</IE>*/

/*<ltIE9>*/
var testForm = document.createElement('form');
testForm.innerHTML = '<select><option>s</option></select>';

if (testForm.firstChild.value != 's') Element.Properties.value = {

	set: function(value){
		var tag = this.get('tag');
		if (tag != 'select') return this.setProperty('value', value);
		var options = this.getElements('option');
		value = String(value);
		for (var i = 0; i < options.length; i++){
			var option = options[i],
				attr = option.getAttributeNode('value'),
				optionValue = (attr && attr.specified) ? option.value : option.get('text');
			if (optionValue === value) return option.selected = true;
		}
	},

	get: function(){
		var option = this, tag = option.get('tag');

		if (tag != 'select' && tag != 'option') return this.getProperty('value');

		if (tag == 'select' && !(option = option.getSelected()[0])) return '';

		var attr = option.getAttributeNode('value');
		return (attr && attr.specified) ? option.value : option.get('text');
	}

};
testForm = null;
/*</ltIE9>*/

/*<IE>*/
if (document.createElement('div').getAttributeNode('id')) Element.Properties.id = {
	set: function(id){
		this.id = this.getAttributeNode('id').value = id;
	},
	get: function(){
		return this.id || null;
	},
	erase: function(){
		this.id = this.getAttributeNode('id').value = '';
	}
};
/*</IE>*/

})();

/*
---

name: Event

description: Contains the Event Type, to make the event object cross-browser.

license: MIT-style license.

requires: [Window, Document, Array, Function, String, Object]

provides: Event

...
*/

(function(){

var _keys = {};
var normalizeWheelSpeed = function(event){
	var normalized;
	if (event.wheelDelta){
		normalized = event.wheelDelta % 120 == 0 ? event.wheelDelta / 120 : event.wheelDelta / 12;
	} else {
		var rawAmount = event.deltaY || event.detail || 0;
		normalized = -(rawAmount % 3 == 0 ? rawAmount / 3 : rawAmount * 10);
	}
	return normalized;
};

var DOMEvent = this.DOMEvent = new Type('DOMEvent', function(event, win){
	if (!win) win = window;
	event = event || win.event;
	if (event.$extended) return event;
	this.event = event;
	this.$extended = true;
	this.shift = event.shiftKey;
	this.control = event.ctrlKey;
	this.alt = event.altKey;
	this.meta = event.metaKey;
	var type = this.type = event.type;
	var target = event.target || event.srcElement;
	while (target && target.nodeType == 3) target = target.parentNode;
	this.target = document.id(target);

	if (type.indexOf('key') == 0){
		var code = this.code = (event.which || event.keyCode);
		if (!this.shift || type != 'keypress') this.key = _keys[code];
		if (type == 'keydown' || type == 'keyup'){
			if (code > 111 && code < 124) this.key = 'f' + (code - 111);
			else if (code > 95 && code < 106) this.key = code - 96;
		}
		if (this.key == null) this.key = String.fromCharCode(code).toLowerCase();
	} else if (type == 'click' || type == 'dblclick' || type == 'contextmenu' || type == 'wheel' || type == 'DOMMouseScroll' || type.indexOf('mouse') == 0){
		var doc = win.document;
		doc = (!doc.compatMode || doc.compatMode == 'CSS1Compat') ? doc.html : doc.body;
		this.page = {
			x: (event.pageX != null) ? event.pageX : event.clientX + doc.scrollLeft,
			y: (event.pageY != null) ? event.pageY : event.clientY + doc.scrollTop
		};
		this.client = {
			x: (event.pageX != null) ? event.pageX - win.pageXOffset : event.clientX,
			y: (event.pageY != null) ? event.pageY - win.pageYOffset : event.clientY
		};
		if (type == 'DOMMouseScroll' || type == 'wheel' || type == 'mousewheel') this.wheel = normalizeWheelSpeed(event);
		this.rightClick = (event.which == 3 || event.button == 2);
		if (type == 'mouseover' || type == 'mouseout' || type == 'mouseenter' || type == 'mouseleave'){
			var overTarget = type == 'mouseover' || type == 'mouseenter';
			var related = event.relatedTarget || event[(overTarget ? 'from' : 'to') + 'Element'];
			while (related && related.nodeType == 3) related = related.parentNode;
			this.relatedTarget = document.id(related);
		}
	} else if (type.indexOf('touch') == 0 || type.indexOf('gesture') == 0){
		this.rotation = event.rotation;
		this.scale = event.scale;
		this.targetTouches = event.targetTouches;
		this.changedTouches = event.changedTouches;
		var touches = this.touches = event.touches;
		if (touches && touches[0]){
			var touch = touches[0];
			this.page = {x: touch.pageX, y: touch.pageY};
			this.client = {x: touch.clientX, y: touch.clientY};
		}
	}

	if (!this.client) this.client = {};
	if (!this.page) this.page = {};
});

DOMEvent.implement({

	stop: function(){
		return this.preventDefault().stopPropagation();
	},

	stopPropagation: function(){
		if (this.event.stopPropagation) this.event.stopPropagation();
		else this.event.cancelBubble = true;
		return this;
	},

	preventDefault: function(){
		if (this.event.preventDefault) this.event.preventDefault();
		else this.event.returnValue = false;
		return this;
	}

});

DOMEvent.defineKey = function(code, key){
	_keys[code] = key;
	return this;
};

DOMEvent.defineKeys = DOMEvent.defineKey.overloadSetter(true);

DOMEvent.defineKeys({
	'38': 'up', '40': 'down', '37': 'left', '39': 'right',
	'27': 'esc', '32': 'space', '8': 'backspace', '9': 'tab',
	'46': 'delete', '13': 'enter'
});

})();





/*
---

name: Element.Event

description: Contains Element methods for dealing with events. This file also includes mouseenter and mouseleave custom Element Events, if necessary.

license: MIT-style license.

requires: [Element, Event]

provides: Element.Event

...
*/

(function(){

Element.Properties.events = {set: function(events){
	this.addEvents(events);
}};

[Element, Window, Document].invoke('implement', {

	addEvent: function(type, fn){
		var events = this.retrieve('events', {});
		if (!events[type]) events[type] = {keys: [], values: []};
		if (events[type].keys.contains(fn)) return this;
		events[type].keys.push(fn);
		var realType = type,
			custom = Element.Events[type],
			condition = fn,
			self = this;
		if (custom){
			if (custom.onAdd) custom.onAdd.call(this, fn, type);
			if (custom.condition){
				condition = function(event){
					if (custom.condition.call(this, event, type)) return fn.call(this, event);
					return true;
				};
			}
			if (custom.base) realType = Function.convert(custom.base).call(this, type);
		}
		var defn = function(){
			return fn.call(self);
		};
		var nativeEvent = Element.NativeEvents[realType];
		if (nativeEvent){
			if (nativeEvent == 2){
				defn = function(event){
					event = new DOMEvent(event, self.getWindow());
					if (condition.call(self, event) === false) event.stop();
				};
			}
			this.addListener(realType, defn, arguments[2]);
		}
		events[type].values.push(defn);
		return this;
	},

	removeEvent: function(type, fn){
		var events = this.retrieve('events');
		if (!events || !events[type]) return this;
		var list = events[type];
		var index = list.keys.indexOf(fn);
		if (index == -1) return this;
		var value = list.values[index];
		delete list.keys[index];
		delete list.values[index];
		var custom = Element.Events[type];
		if (custom){
			if (custom.onRemove) custom.onRemove.call(this, fn, type);
			if (custom.base) type = Function.convert(custom.base).call(this, type);
		}
		return (Element.NativeEvents[type]) ? this.removeListener(type, value, arguments[2]) : this;
	},

	addEvents: function(events){
		for (var event in events) this.addEvent(event, events[event]);
		return this;
	},

	removeEvents: function(events){
		var type;
		if (typeOf(events) == 'object'){
			for (type in events) this.removeEvent(type, events[type]);
			return this;
		}
		var attached = this.retrieve('events');
		if (!attached) return this;
		if (!events){
			for (type in attached) this.removeEvents(type);
			this.eliminate('events');
		} else if (attached[events]){
			attached[events].keys.each(function(fn){
				this.removeEvent(events, fn);
			}, this);
			delete attached[events];
		}
		return this;
	},

	fireEvent: function(type, args, delay){
		var events = this.retrieve('events');
		if (!events || !events[type]) return this;
		args = Array.convert(args);

		events[type].keys.each(function(fn){
			if (delay) fn.delay(delay, this, args);
			else fn.apply(this, args);
		}, this);
		return this;
	},

	cloneEvents: function(from, type){
		from = document.id(from);
		var events = from.retrieve('events');
		if (!events) return this;
		if (!type){
			for (var eventType in events) this.cloneEvents(from, eventType);
		} else if (events[type]){
			events[type].keys.each(function(fn){
				this.addEvent(type, fn);
			}, this);
		}
		return this;
	}

});

Element.NativeEvents = {
	click: 2, dblclick: 2, mouseup: 2, mousedown: 2, contextmenu: 2, //mouse buttons
	wheel: 2, mousewheel: 2, DOMMouseScroll: 2, //mouse wheel
	mouseover: 2, mouseout: 2, mousemove: 2, selectstart: 2, selectend: 2, //mouse movement
	keydown: 2, keypress: 2, keyup: 2, //keyboard
	orientationchange: 2, // mobile
	touchstart: 2, touchmove: 2, touchend: 2, touchcancel: 2, // touch
	gesturestart: 2, gesturechange: 2, gestureend: 2, // gesture
	focus: 2, blur: 2, change: 2, reset: 2, select: 2, submit: 2, paste: 2, input: 2, //form elements
	load: 2, unload: 1, beforeunload: 2, resize: 1, move: 1, DOMContentLoaded: 1, readystatechange: 1, //window
	hashchange: 1, popstate: 2, pageshow: 2, pagehide: 2, // history
	error: 1, abort: 1, scroll: 1, message: 2 //misc
};

Element.Events = {
	mousewheel: {
		base: 'onwheel' in document ? 'wheel' : 'onmousewheel' in document ? 'mousewheel' : 'DOMMouseScroll'
	}
};

var check = function(event){
	var related = event.relatedTarget;
	if (related == null) return true;
	if (!related) return false;
	return (related != this && related.prefix != 'xul' && typeOf(this) != 'document' && !this.contains(related));
};

if ('onmouseenter' in document.documentElement){
	Element.NativeEvents.mouseenter = Element.NativeEvents.mouseleave = 2;
	Element.MouseenterCheck = check;
} else {
	Element.Events.mouseenter = {
		base: 'mouseover',
		condition: check
	};

	Element.Events.mouseleave = {
		base: 'mouseout',
		condition: check
	};
}

/*<ltIE9>*/
if (!window.addEventListener){
	Element.NativeEvents.propertychange = 2;
	Element.Events.change = {
		base: function(){
			var type = this.type;
			return (this.get('tag') == 'input' && (type == 'radio' || type == 'checkbox')) ? 'propertychange' : 'change';
		},
		condition: function(event){
			return event.type != 'propertychange' || event.event.propertyName == 'checked';
		}
	};
}
/*</ltIE9>*/



})();

/*
---

name: Element.Delegation

description: Extends the Element native object to include the delegate method for more efficient event management.

license: MIT-style license.

requires: [Element.Event]

provides: [Element.Delegation]

...
*/

(function(){

var eventListenerSupport = !!window.addEventListener;

Element.NativeEvents.focusin = Element.NativeEvents.focusout = 2;

var bubbleUp = function(self, match, fn, event, target){
	while (target && target != self){
		if (match(target, event)) return fn.call(target, event, target);
		target = document.id(target.parentNode);
	}
};

var map = {
	mouseenter: {
		base: 'mouseover',
		condition: Element.MouseenterCheck
	},
	mouseleave: {
		base: 'mouseout',
		condition: Element.MouseenterCheck
	},
	focus: {
		base: 'focus' + (eventListenerSupport ? '' : 'in'),
		capture: true
	},
	blur: {
		base: eventListenerSupport ? 'blur' : 'focusout',
		capture: true
	}
};

/*<ltIE9>*/
var _key = '$delegation:';
var formObserver = function(type){

	return {

		base: 'focusin',

		remove: function(self, uid){
			var list = self.retrieve(_key + type + 'listeners', {})[uid];
			if (list && list.forms) for (var i = list.forms.length; i--;){
				// the form may have been destroyed, so it won't have the
				// removeEvent method anymore. In that case the event was
				// removed as well.
				if (list.forms[i].removeEvent) list.forms[i].removeEvent(type, list.fns[i]);
			}
		},

		listen: function(self, match, fn, event, target, uid){
			var form = (target.get('tag') == 'form') ? target : event.target.getParent('form');
			if (!form) return;

			var listeners = self.retrieve(_key + type + 'listeners', {}),
				listener = listeners[uid] || {forms: [], fns: []},
				forms = listener.forms, fns = listener.fns;

			if (forms.indexOf(form) != -1) return;
			forms.push(form);

			var _fn = function(event){
				bubbleUp(self, match, fn, event, target);
			};
			form.addEvent(type, _fn);
			fns.push(_fn);

			listeners[uid] = listener;
			self.store(_key + type + 'listeners', listeners);
		}
	};
};

var inputObserver = function(type){
	return {
		base: 'focusin',
		listen: function(self, match, fn, event, target){
			var events = {blur: function(){
				this.removeEvents(events);
			}};
			events[type] = function(event){
				bubbleUp(self, match, fn, event, target);
			};
			event.target.addEvents(events);
		}
	};
};

if (!eventListenerSupport) Object.append(map, {
	submit: formObserver('submit'),
	reset: formObserver('reset'),
	change: inputObserver('change'),
	select: inputObserver('select')
});
/*</ltIE9>*/

var proto = Element.prototype,
	addEvent = proto.addEvent,
	removeEvent = proto.removeEvent;

var relay = function(old, method){
	return function(type, fn, useCapture){
		if (type.indexOf(':relay') == -1) return old.call(this, type, fn, useCapture);
		var parsed = Slick.parse(type).expressions[0][0];
		if (parsed.pseudos[0].key != 'relay') return old.call(this, type, fn, useCapture);
		var newType = parsed.tag;
		parsed.pseudos.slice(1).each(function(pseudo){
			newType += ':' + pseudo.key + (pseudo.value ? '(' + pseudo.value + ')' : '');
		});
		old.call(this, type, fn);
		return method.call(this, newType, parsed.pseudos[0].value, fn);
	};
};

var delegation = {

	addEvent: function(type, match, fn){
		var storage = this.retrieve('$delegates', {}), stored = storage[type];
		if (stored) for (var _uid in stored){
			if (stored[_uid].fn == fn && stored[_uid].match == match) return this;
		}

		var _type = type, _match = match, _fn = fn, _map = map[type] || {};
		type = _map.base || _type;

		match = function(target){
			return Slick.match(target, _match);
		};

		var elementEvent = Element.Events[_type];
		if (_map.condition || elementEvent && elementEvent.condition){
			var __match = match, condition = _map.condition || elementEvent.condition;
			match = function(target, event){
				return __match(target, event) && condition.call(target, event, type);
			};
		}

		var self = this, uid = String.uniqueID();
		var delegator = _map.listen ? function(event, target){
			if (!target && event && event.target) target = event.target;
			if (target) _map.listen(self, match, fn, event, target, uid);
		} : function(event, target){
			if (!target && event && event.target) target = event.target;
			if (target) bubbleUp(self, match, fn, event, target);
		};

		if (!stored) stored = {};
		stored[uid] = {
			match: _match,
			fn: _fn,
			delegator: delegator
		};
		storage[_type] = stored;
		return addEvent.call(this, type, delegator, _map.capture);
	},

	removeEvent: function(type, match, fn, _uid){
		var storage = this.retrieve('$delegates', {}), stored = storage[type];
		if (!stored) return this;

		if (_uid){
			var _type = type, delegator = stored[_uid].delegator, _map = map[type] || {};
			type = _map.base || _type;
			if (_map.remove) _map.remove(this, _uid);
			delete stored[_uid];
			storage[_type] = stored;
			return removeEvent.call(this, type, delegator, _map.capture);
		}

		var __uid, s;
		if (fn) for (__uid in stored){
			s = stored[__uid];
			if (s.match == match && s.fn == fn) return delegation.removeEvent.call(this, type, match, fn, __uid);
		} else for (__uid in stored){
			s = stored[__uid];
			if (s.match == match) delegation.removeEvent.call(this, type, match, s.fn, __uid);
		}
		return this;
	}

};

[Element, Window, Document].invoke('implement', {
	addEvent: relay(addEvent, delegation.addEvent),
	removeEvent: relay(removeEvent, delegation.removeEvent)
});

})();

/*
---

name: Element.Style

description: Contains methods for interacting with the styles of Elements in a fashionable way.

license: MIT-style license.

requires: Element

provides: Element.Style

...
*/

(function(){

var html = document.html, el;

//<ltIE9>
// Check for oldIE, which does not remove styles when they're set to null
el = document.createElement('div');
el.style.color = 'red';
el.style.color = null;
var doesNotRemoveStyles = el.style.color == 'red';

// check for oldIE, which returns border* shorthand styles in the wrong order (color-width-style instead of width-style-color)
var border = '1px solid #123abc';
el.style.border = border;
var returnsBordersInWrongOrder = el.style.border != border;
el = null;
//</ltIE9>

var hasGetComputedStyle = !!window.getComputedStyle,
	supportBorderRadius = document.createElement('div').style.borderRadius != null;

Element.Properties.styles = {set: function(styles){
	this.setStyles(styles);
}};

var hasOpacity = (html.style.opacity != null),
	hasFilter = (html.style.filter != null),
	reAlpha = /alpha\(opacity=([\d.]+)\)/i;

var setVisibility = function(element, opacity){
	element.store('$opacity', opacity);
	element.style.visibility = opacity > 0 || opacity == null ? 'visible' : 'hidden';
};

//<ltIE9>
var setFilter = function(element, regexp, value){
	var style = element.style,
		filter = style.filter || element.getComputedStyle('filter') || '';
	style.filter = (regexp.test(filter) ? filter.replace(regexp, value) : filter + ' ' + value).trim();
	if (!style.filter) style.removeAttribute('filter');
};
//</ltIE9>

var setOpacity = (hasOpacity ? function(element, opacity){
	element.style.opacity = opacity;
} : (hasFilter ? function(element, opacity){
	if (!element.currentStyle || !element.currentStyle.hasLayout) element.style.zoom = 1;
	if (opacity == null || opacity == 1){
		setFilter(element, reAlpha, '');
		if (opacity == 1 && getOpacity(element) != 1) setFilter(element, reAlpha, 'alpha(opacity=100)');
	} else {
		setFilter(element, reAlpha, 'alpha(opacity=' + (opacity * 100).limit(0, 100).round() + ')');
	}
} : setVisibility));

var getOpacity = (hasOpacity ? function(element){
	var opacity = element.style.opacity || element.getComputedStyle('opacity');
	return (opacity == '') ? 1 : opacity.toFloat();
} : (hasFilter ? function(element){
	var filter = (element.style.filter || element.getComputedStyle('filter')),
		opacity;
	if (filter) opacity = filter.match(reAlpha);
	return (opacity == null || filter == null) ? 1 : (opacity[1] / 100);
} : function(element){
	var opacity = element.retrieve('$opacity');
	if (opacity == null) opacity = (element.style.visibility == 'hidden' ? 0 : 1);
	return opacity;
}));

var floatName = (html.style.cssFloat == null) ? 'styleFloat' : 'cssFloat',
	namedPositions = {left: '0%', top: '0%', center: '50%', right: '100%', bottom: '100%'},
	hasBackgroundPositionXY = (html.style.backgroundPositionX != null),
	prefixPattern = /^-(ms)-/;

var camelCase = function(property){
	return property.replace(prefixPattern, '$1-').camelCase();
};

//<ltIE9>
var removeStyle = function(style, property){
	if (property == 'backgroundPosition'){
		style.removeAttribute(property + 'X');
		property += 'Y';
	}
	style.removeAttribute(property);
};
//</ltIE9>

Element.implement({

	getComputedStyle: function(property){
		if (!hasGetComputedStyle && this.currentStyle) return this.currentStyle[camelCase(property)];
		var defaultView = Element.getDocument(this).defaultView,
			computed = defaultView ? defaultView.getComputedStyle(this, null) : null;
		return (computed) ? computed.getPropertyValue((property == floatName) ? 'float' : property.hyphenate()) : '';
	},

	setStyle: function(property, value){
		if (property == 'opacity'){
			if (value != null) value = parseFloat(value);
			setOpacity(this, value);
			return this;
		}
		property = camelCase(property == 'float' ? floatName : property);
		if (typeOf(value) != 'string'){
			var map = (Element.Styles[property] || '@').split(' ');
			value = Array.convert(value).map(function(val, i){
				if (!map[i]) return '';
				return (typeOf(val) == 'number') ? map[i].replace('@', Math.round(val)) : val;
			}).join(' ');
		} else if (value == String(Number(value))){
			value = Math.round(value);
		}
		this.style[property] = value;
		//<ltIE9>
		if ((value == '' || value == null) && doesNotRemoveStyles && this.style.removeAttribute){
			removeStyle(this.style, property);
		}
		//</ltIE9>
		return this;
	},

	getStyle: function(property){
		if (property == 'opacity') return getOpacity(this);
		property = camelCase(property == 'float' ? floatName : property);
		if (supportBorderRadius && property.indexOf('borderRadius') != -1){
			return ['borderTopLeftRadius', 'borderTopRightRadius', 'borderBottomRightRadius', 'borderBottomLeftRadius'].map(function(corner){
				return this.style[corner] || '0px';
			}, this).join(' ');
		}
		var result = this.style[property];
		if (!result || property == 'zIndex'){
			if (Element.ShortStyles.hasOwnProperty(property)){
				result = [];
				for (var s in Element.ShortStyles[property]) result.push(this.getStyle(s));
				return result.join(' ');
			}
			result = this.getComputedStyle(property);
		}
		if (hasBackgroundPositionXY && /^backgroundPosition[XY]?$/.test(property)){
			return result.replace(/(top|right|bottom|left)/g, function(position){
				return namedPositions[position];
			}) || '0px';
		}
		if (!result && property == 'backgroundPosition') return '0px 0px';
		if (result){
			result = String(result);
			var color = result.match(/rgba?\([\d\s,]+\)/);
			if (color) result = result.replace(color[0], color[0].rgbToHex());
		}
		if (!hasGetComputedStyle && !this.style[property]){
			if ((/^(height|width)$/).test(property) && !(/px$/.test(result))){
				var values = (property == 'width') ? ['left', 'right'] : ['top', 'bottom'], size = 0;
				values.each(function(value){
					size += this.getStyle('border-' + value + '-width').toInt() + this.getStyle('padding-' + value).toInt();
				}, this);
				return this['offset' + property.capitalize()] - size + 'px';
			}
			if ((/^border(.+)Width|margin|padding/).test(property) && isNaN(parseFloat(result))){
				return '0px';
			}
		}
		//<ltIE9>
		if (returnsBordersInWrongOrder && /^border(Top|Right|Bottom|Left)?$/.test(property) && /^#/.test(result)){
			return result.replace(/^(.+)\s(.+)\s(.+)$/, '$2 $3 $1');
		}
		//</ltIE9>

		return result;
	},

	setStyles: function(styles){
		for (var style in styles) this.setStyle(style, styles[style]);
		return this;
	},

	getStyles: function(){
		var result = {};
		Array.flatten(arguments).each(function(key){
			result[key] = this.getStyle(key);
		}, this);
		return result;
	}

});

Element.Styles = {
	left: '@px', top: '@px', bottom: '@px', right: '@px',
	width: '@px', height: '@px', maxWidth: '@px', maxHeight: '@px', minWidth: '@px', minHeight: '@px',
	backgroundColor: 'rgb(@, @, @)', backgroundSize: '@px', backgroundPosition: '@px @px', color: 'rgb(@, @, @)',
	fontSize: '@px', letterSpacing: '@px', lineHeight: '@px', clip: 'rect(@px @px @px @px)',
	margin: '@px @px @px @px', padding: '@px @px @px @px', border: '@px @ rgb(@, @, @) @px @ rgb(@, @, @) @px @ rgb(@, @, @)',
	borderWidth: '@px @px @px @px', borderStyle: '@ @ @ @', borderColor: 'rgb(@, @, @) rgb(@, @, @) rgb(@, @, @) rgb(@, @, @)',
	zIndex: '@', 'zoom': '@', fontWeight: '@', textIndent: '@px', opacity: '@', borderRadius: '@px @px @px @px'
};





Element.ShortStyles = {margin: {}, padding: {}, border: {}, borderWidth: {}, borderStyle: {}, borderColor: {}};

['Top', 'Right', 'Bottom', 'Left'].each(function(direction){
	var Short = Element.ShortStyles;
	var All = Element.Styles;
	['margin', 'padding'].each(function(style){
		var sd = style + direction;
		Short[style][sd] = All[sd] = '@px';
	});
	var bd = 'border' + direction;
	Short.border[bd] = All[bd] = '@px @ rgb(@, @, @)';
	var bdw = bd + 'Width', bds = bd + 'Style', bdc = bd + 'Color';
	Short[bd] = {};
	Short.borderWidth[bdw] = Short[bd][bdw] = All[bdw] = '@px';
	Short.borderStyle[bds] = Short[bd][bds] = All[bds] = '@';
	Short.borderColor[bdc] = Short[bd][bdc] = All[bdc] = 'rgb(@, @, @)';
});

if (hasBackgroundPositionXY) Element.ShortStyles.backgroundPosition = {backgroundPositionX: '@', backgroundPositionY: '@'};
})();

/*
---

name: Element.Dimensions

description: Contains methods to work with size, scroll, or positioning of Elements and the window object.

license: MIT-style license.

credits:
  - Element positioning based on the [qooxdoo](http://qooxdoo.org/) code and smart browser fixes, [LGPL License](http://www.gnu.org/licenses/lgpl.html).
  - Viewport dimensions based on [YUI](http://developer.yahoo.com/yui/) code, [BSD License](http://developer.yahoo.com/yui/license.html).

requires: [Element, Element.Style]

provides: [Element.Dimensions]

...
*/

(function(){

var element = document.createElement('div'),
	child = document.createElement('div');
element.style.height = '0';
element.appendChild(child);
var brokenOffsetParent = (child.offsetParent === element);
element = child = null;

var heightComponents = ['height', 'paddingTop', 'paddingBottom', 'borderTopWidth', 'borderBottomWidth'],
	widthComponents = ['width', 'paddingLeft', 'paddingRight', 'borderLeftWidth', 'borderRightWidth'];

var svgCalculateSize = function(el){

	var gCS = window.getComputedStyle(el),
		bounds = {x: 0, y: 0};

	heightComponents.each(function(css){
		bounds.y += parseFloat(gCS[css]);
	});
	widthComponents.each(function(css){
		bounds.x += parseFloat(gCS[css]);
	});
	return bounds;
};

var isOffset = function(el){
	return styleString(el, 'position') != 'static' || isBody(el);
};

var isOffsetStatic = function(el){
	return isOffset(el) || (/^(?:table|td|th)$/i).test(el.tagName);
};

Element.implement({

	scrollTo: function(x, y){
		if (isBody(this)){
			this.getWindow().scrollTo(x, y);
		} else {
			this.scrollLeft = x;
			this.scrollTop = y;
		}
		return this;
	},

	getSize: function(){
		if (isBody(this)) return this.getWindow().getSize();

		//<ltIE9>
		// This if clause is because IE8- cannot calculate getBoundingClientRect of elements with visibility hidden.
		if (!window.getComputedStyle) return {x: this.offsetWidth, y: this.offsetHeight};
		//</ltIE9>

		// This svg section under, calling `svgCalculateSize()`, can be removed when FF fixed the svg size bug.
		// Bug info: https://bugzilla.mozilla.org/show_bug.cgi?id=530985
		if (this.get('tag') == 'svg') return svgCalculateSize(this);

		try {
			var bounds = this.getBoundingClientRect();
			return {x: bounds.width, y: bounds.height};
		} catch (e){
			return {x: 0, y: 0};
		}
	},

	getScrollSize: function(){
		if (isBody(this)) return this.getWindow().getScrollSize();
		return {x: this.scrollWidth, y: this.scrollHeight};
	},

	getScroll: function(){
		if (isBody(this)) return this.getWindow().getScroll();
		return {x: this.scrollLeft, y: this.scrollTop};
	},

	getScrolls: function(){
		var element = this.parentNode, position = {x: 0, y: 0};
		while (element && !isBody(element)){
			position.x += element.scrollLeft;
			position.y += element.scrollTop;
			element = element.parentNode;
		}
		return position;
	},

	getOffsetParent: brokenOffsetParent ? function(){
		var element = this;
		if (isBody(element) || styleString(element, 'position') == 'fixed') return null;

		var isOffsetCheck = (styleString(element, 'position') == 'static') ? isOffsetStatic : isOffset;
		while ((element = element.parentNode)){
			if (isOffsetCheck(element)) return element;
		}
		return null;
	} : function(){
		var element = this;
		if (isBody(element) || styleString(element, 'position') == 'fixed') return null;

		try {
			return element.offsetParent;
		} catch (e){}
		return null;
	},

	getOffsets: function(){
		var hasGetBoundingClientRect = this.getBoundingClientRect;

		if (hasGetBoundingClientRect){
			var bound = this.getBoundingClientRect(),
				html = document.id(this.getDocument().documentElement),
				htmlScroll = html.getScroll(),
				elemScrolls = this.getScrolls(),
				isFixed = (styleString(this, 'position') == 'fixed');

			return {
				x: bound.left.toFloat() + elemScrolls.x + ((isFixed) ? 0 : htmlScroll.x) - html.clientLeft,
				y: bound.top.toFloat() + elemScrolls.y + ((isFixed) ? 0 : htmlScroll.y) - html.clientTop
			};
		}

		var element = this, position = {x: 0, y: 0};
		if (isBody(this)) return position;

		while (element && !isBody(element)){
			position.x += element.offsetLeft;
			position.y += element.offsetTop;

			element = element.offsetParent;
		}

		return position;
	},

	getPosition: function(relative){
		var offset = this.getOffsets(),
			scroll = this.getScrolls();
		var position = {
			x: offset.x - scroll.x,
			y: offset.y - scroll.y
		};

		if (relative && (relative = document.id(relative))){
			var relativePosition = relative.getPosition();
			return {x: position.x - relativePosition.x - leftBorder(relative), y: position.y - relativePosition.y - topBorder(relative)};
		}
		return position;
	},

	getCoordinates: function(element){
		if (isBody(this)) return this.getWindow().getCoordinates();
		var position = this.getPosition(element),
			size = this.getSize();
		var obj = {
			left: position.x,
			top: position.y,
			width: size.x,
			height: size.y
		};
		obj.right = obj.left + obj.width;
		obj.bottom = obj.top + obj.height;
		return obj;
	},

	computePosition: function(obj){
		return {
			left: obj.x - styleNumber(this, 'margin-left'),
			top: obj.y - styleNumber(this, 'margin-top')
		};
	},

	setPosition: function(obj){
		return this.setStyles(this.computePosition(obj));
	}

});


[Document, Window].invoke('implement', {

	getSize: function(){
		var doc = getCompatElement(this);
		return {x: doc.clientWidth, y: doc.clientHeight};
	},

	getScroll: function(){
		var win = this.getWindow(), doc = getCompatElement(this);
		return {x: win.pageXOffset || doc.scrollLeft, y: win.pageYOffset || doc.scrollTop};
	},

	getScrollSize: function(){
		var doc = getCompatElement(this),
			min = this.getSize(),
			body = this.getDocument().body;

		return {x: Math.max(doc.scrollWidth, body.scrollWidth, min.x), y: Math.max(doc.scrollHeight, body.scrollHeight, min.y)};
	},

	getPosition: function(){
		return {x: 0, y: 0};
	},

	getCoordinates: function(){
		var size = this.getSize();
		return {top: 0, left: 0, bottom: size.y, right: size.x, height: size.y, width: size.x};
	}

});

// private methods

var styleString = Element.getComputedStyle;

function styleNumber(element, style){
	return styleString(element, style).toInt() || 0;
}



function topBorder(element){
	return styleNumber(element, 'border-top-width');
}

function leftBorder(element){
	return styleNumber(element, 'border-left-width');
}

function isBody(element){
	return (/^(?:body|html)$/i).test(element.tagName);
}

function getCompatElement(element){
	var doc = element.getDocument();
	return (!doc.compatMode || doc.compatMode == 'CSS1Compat') ? doc.html : doc.body;
}

})();

//aliases
Element.alias({position: 'setPosition'}); //compatability

[Window, Document, Element].invoke('implement', {

	getHeight: function(){
		return this.getSize().y;
	},

	getWidth: function(){
		return this.getSize().x;
	},

	getScrollTop: function(){
		return this.getScroll().y;
	},

	getScrollLeft: function(){
		return this.getScroll().x;
	},

	getScrollHeight: function(){
		return this.getScrollSize().y;
	},

	getScrollWidth: function(){
		return this.getScrollSize().x;
	},

	getTop: function(){
		return this.getPosition().y;
	},

	getLeft: function(){
		return this.getPosition().x;
	}

});

/*
---

name: Request

description: Powerful all purpose Request Class. Uses XMLHTTPRequest.

license: MIT-style license.

requires: [Object, Element, Chain, Events, Options, Class.Thenable, Browser]

provides: Request

...
*/

(function(){

var empty = function(){},
	progressSupport = ('onprogress' in new Browser.Request);

var Request = this.Request = new Class({

	Implements: [Chain, Events, Options, Class.Thenable],

	options: {/*
		onRequest: function(){},
		onLoadstart: function(event, xhr){},
		onProgress: function(event, xhr){},
		onComplete: function(){},
		onCancel: function(){},
		onSuccess: function(responseText, responseXML){},
		onFailure: function(xhr){},
		onException: function(headerName, value){},
		onTimeout: function(){},
		user: '',
		password: '',
		withCredentials: false,*/
		url: '',
		data: '',
		headers: {
			'X-Requested-With': 'XMLHttpRequest',
			'Accept': 'text/javascript, text/html, application/xml, text/xml, */*'
		},
		async: true,
		format: false,
		method: 'post',
		link: 'ignore',
		isSuccess: null,
		emulation: true,
		urlEncoded: true,
		encoding: 'utf-8',
		evalScripts: false,
		evalResponse: false,
		timeout: 0,
		noCache: false
	},

	initialize: function(options){
		this.xhr = new Browser.Request();
		this.setOptions(options);
		this.headers = this.options.headers;
	},

	onStateChange: function(){
		var xhr = this.xhr;
		if (xhr.readyState != 4 || !this.running) return;
		this.running = false;
		this.status = 0;
		Function.attempt(function(){
			var status = xhr.status;
			this.status = (status == 1223) ? 204 : status;
		}.bind(this));
		xhr.onreadystatechange = empty;
		if (progressSupport) xhr.onprogress = xhr.onloadstart = empty;
		if (this.timer){
			clearTimeout(this.timer);
			delete this.timer;
		}

		this.response = {text: this.xhr.responseText || '', xml: this.xhr.responseXML};
		if (this.options.isSuccess.call(this, this.status))
			this.success(this.response.text, this.response.xml);
		else
			this.failure();
	},

	isSuccess: function(){
		var status = this.status;
		return (status >= 200 && status < 300);
	},

	isRunning: function(){
		return !!this.running;
	},

	processScripts: function(text){
		if (this.options.evalResponse || (/(ecma|java)script/).test(this.getHeader('Content-type'))) return Browser.exec(text);
		return text.stripScripts(this.options.evalScripts);
	},

	success: function(text, xml){
		this.onSuccess(this.processScripts(text), xml);
		this.resolve({text: text, xml: xml});
	},

	onSuccess: function(){
		this.fireEvent('complete', arguments).fireEvent('success', arguments).callChain();
	},

	failure: function(){
		this.onFailure();
		this.reject({reason: 'failure', xhr: this.xhr});
	},

	onFailure: function(){
		this.fireEvent('complete').fireEvent('failure', this.xhr);
	},

	loadstart: function(event){
		this.fireEvent('loadstart', [event, this.xhr]);
	},

	progress: function(event){
		this.fireEvent('progress', [event, this.xhr]);
	},

	timeout: function(){
		this.fireEvent('timeout', this.xhr);
		this.reject({reason: 'timeout', xhr: this.xhr});
	},

	setHeader: function(name, value){
		this.headers[name] = value;
		return this;
	},

	getHeader: function(name){
		return Function.attempt(function(){
			return this.xhr.getResponseHeader(name);
		}.bind(this));
	},

	check: function(){
		if (!this.running) return true;
		switch (this.options.link){
			case 'cancel': this.cancel(); return true;
			case 'chain': this.chain(this.caller.pass(arguments, this)); return false;
		}
		return false;
	},

	send: function(options){
		if (!this.check(options)) return this;

		this.options.isSuccess = this.options.isSuccess || this.isSuccess;
		this.running = true;

		var type = typeOf(options);
		if (type == 'string' || type == 'element') options = {data: options};

		var old = this.options;
		options = Object.append({data: old.data, url: old.url, method: old.method}, options);
		var data = options.data, url = String(options.url), method = options.method.toLowerCase();

		switch (typeOf(data)){
			case 'element': data = document.id(data).toQueryString(); break;
			case 'object': case 'hash': data = Object.toQueryString(data);
		}

		if (this.options.format){
			var format = 'format=' + this.options.format;
			data = (data) ? format + '&' + data : format;
		}

		if (this.options.emulation && !['get', 'post'].contains(method)){
			var _method = '_method=' + method;
			data = (data) ? _method + '&' + data : _method;
			method = 'post';
		}

		if (this.options.urlEncoded && ['post', 'put'].contains(method)){
			var encoding = (this.options.encoding) ? '; charset=' + this.options.encoding : '';
			this.headers['Content-type'] = 'application/x-www-form-urlencoded' + encoding;
		}

		if (!url) url = document.location.pathname;

		var trimPosition = url.lastIndexOf('/');
		if (trimPosition > -1 && (trimPosition = url.indexOf('#')) > -1) url = url.substr(0, trimPosition);

		if (this.options.noCache)
			url += (url.indexOf('?') > -1 ? '&' : '?') + String.uniqueID();

		if (data && (method == 'get' || method == 'delete')){
			url += (url.indexOf('?') > -1 ? '&' : '?') + data;
			data = null;
		}

		var xhr = this.xhr;
		if (progressSupport){
			xhr.onloadstart = this.loadstart.bind(this);
			xhr.onprogress = this.progress.bind(this);
		}

		xhr.open(method.toUpperCase(), url, this.options.async, this.options.user, this.options.password);
		if ((this.options.withCredentials) && 'withCredentials' in xhr) xhr.withCredentials = true;

		xhr.onreadystatechange = this.onStateChange.bind(this);

		Object.each(this.headers, function(value, key){
			try {
				xhr.setRequestHeader(key, value);
			} catch (e){
				this.fireEvent('exception', [key, value]);
				this.reject({reason: 'exception', xhr: xhr, exception: e});
			}
		}, this);

		if (this.getThenableState() !== 'pending'){
			this.resetThenable({reason: 'send'});
		}
		this.fireEvent('request');
		xhr.send(data);
		if (!this.options.async) this.onStateChange();
		else if (this.options.timeout) this.timer = this.timeout.delay(this.options.timeout, this);
		return this;
	},

	cancel: function(){
		if (!this.running) return this;
		this.running = false;
		var xhr = this.xhr;
		xhr.abort();
		if (this.timer){
			clearTimeout(this.timer);
			delete this.timer;
		}
		xhr.onreadystatechange = empty;
		if (progressSupport) xhr.onprogress = xhr.onloadstart = empty;
		this.xhr = new Browser.Request();
		this.fireEvent('cancel');
		this.reject({reason: 'cancel', xhr: xhr});
		return this;
	}

});

var methods = {};
['get', 'post', 'put', 'delete', 'patch', 'head', 'GET', 'POST', 'PUT', 'DELETE', 'PATCH', 'HEAD'].each(function(method){
	methods[method] = function(data){
		var object = {
			method: method
		};
		if (data != null) object.data = data;
		return this.send(object);
	};
});

Request.implement(methods);

Element.Properties.send = {

	set: function(options){
		var send = this.get('send').cancel();
		send.setOptions(options);
		return this;
	},

	get: function(){
		var send = this.retrieve('send');
		if (!send){
			send = new Request({
				data: this, link: 'cancel', method: this.get('method') || 'post', url: this.get('action')
			});
			this.store('send', send);
		}
		return send;
	}

};

Element.implement({

	send: function(url){
		var sender = this.get('send');
		sender.send({data: this, url: url || sender.options.url});
		return this;
	}

});

})();

/*
---

name: Request.HTML

description: Extends the basic Request Class with additional methods for interacting with HTML responses.

license: MIT-style license.

requires: [Element, Request]

provides: Request.HTML

...
*/

Request.HTML = new Class({

	Extends: Request,

	options: {
		update: false,
		append: false,
		evalScripts: true,
		filter: false,
		headers: {
			Accept: 'text/html, application/xml, text/xml, */*'
		}
	},

	success: function(text){
		var options = this.options, response = this.response;

		response.html = text.stripScripts(function(script){
			response.javascript = script;
		});

		var match = response.html.match(/<body[^>]*>([\s\S]*?)<\/body>/i);
		if (match) response.html = match[1];
		var temp = new Element('div').set('html', response.html);

		response.tree = temp.childNodes;
		response.elements = temp.getElements(options.filter || '*');

		if (options.filter) response.tree = response.elements;
		if (options.update){
			var update = document.id(options.update).empty();
			if (options.filter) update.adopt(response.elements);
			else update.set('html', response.html);
		} else if (options.append){
			var append = document.id(options.append);
			if (options.filter) response.elements.reverse().inject(append);
			else append.adopt(temp.getChildren());
		}
		if (options.evalScripts) Browser.exec(response.javascript);

		this.onSuccess(response.tree, response.elements, response.html, response.javascript);
		this.resolve({tree: response.tree, elements: response.elements, html: response.html, javascript: response.javascript});
	}

});

Element.Properties.load = {

	set: function(options){
		var load = this.get('load').cancel();
		load.setOptions(options);
		return this;
	},

	get: function(){
		var load = this.retrieve('load');
		if (!load){
			load = new Request.HTML({data: this, link: 'cancel', update: this, method: 'get'});
			this.store('load', load);
		}
		return load;
	}

};

Element.implement({

	load: function(){
		this.get('load').send(Array.link(arguments, {data: Type.isObject, url: Type.isString}));
		return this;
	}

});

/*
---

name: JSON

description: JSON encoder and decoder.

license: MIT-style license.

SeeAlso: <http://www.json.org/>

requires: [Array, String, Number, Function]

provides: JSON

...
*/

if (typeof JSON == 'undefined') this.JSON = {};



(function(){

var special = {'\b': '\\b', '\t': '\\t', '\n': '\\n', '\f': '\\f', '\r': '\\r', '"' : '\\"', '\\': '\\\\'};

var escape = function(chr){
	return special[chr] || '\\u' + ('0000' + chr.charCodeAt(0).toString(16)).slice(-4);
};

JSON.validate = function(string){
	string = string.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@').
					replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').
					replace(/(?:^|:|,)(?:\s*\[)+/g, '');

	return (/^[\],:{}\s]*$/).test(string);
};

JSON.encode = JSON.stringify ? function(obj){
	return JSON.stringify(obj);
} : function(obj){
	if (obj && obj.toJSON) obj = obj.toJSON();

	switch (typeOf(obj)){
		case 'string':
			return '"' + obj.replace(/[\x00-\x1f\\"]/g, escape) + '"';
		case 'array':
			return '[' + obj.map(JSON.encode).clean() + ']';
		case 'object': case 'hash':
			var string = [];
			Object.each(obj, function(value, key){
				var json = JSON.encode(value);
				if (json) string.push(JSON.encode(key) + ':' + json);
			});
			return '{' + string + '}';
		case 'number': case 'boolean': return '' + obj;
		case 'null': return 'null';
	}

	return null;
};

JSON.secure = true;


JSON.decode = function(string, secure){
	if (!string || typeOf(string) != 'string') return null;

	if (secure == null) secure = JSON.secure;
	if (secure){
		if (JSON.parse) return JSON.parse(string);
		if (!JSON.validate(string)) throw new Error('JSON could not decode the input; security is enabled and the value is not secure.');
	}

	return eval('(' + string + ')');
};

})();

/*
---

name: Request.JSON

description: Extends the basic Request Class with additional methods for sending and receiving JSON data.

license: MIT-style license.

requires: [Request, JSON]

provides: Request.JSON

...
*/

Request.JSON = new Class({

	Extends: Request,

	options: {
		/*onError: function(text, error){},*/
		secure: true
	},

	initialize: function(options){
		this.parent(options);
		Object.append(this.headers, {
			'Accept': 'application/json',
			'X-Request': 'JSON'
		});
	},

	success: function(text){
		var json;
		try {
			json = this.response.json = JSON.decode(text, this.options.secure);
		} catch (error){
			this.fireEvent('error', [text, error]);
			return;
		}
		if (json == null){
			this.failure();
		} else {
			this.onSuccess(json, text);
			this.resolve({json: json, text: text});
		}
	}

});

/*
---

name: Cookie

description: Class for creating, reading, and deleting browser Cookies.

license: MIT-style license.

credits:
  - Based on the functions by Peter-Paul Koch (http://quirksmode.org).

requires: [Options, Browser]

provides: Cookie

...
*/

var Cookie = new Class({

	Implements: Options,

	options: {
		path: '/',
		domain: false,
		duration: false,
		secure: false,
		document: document,
		encode: true,
		httpOnly: false
	},

	initialize: function(key, options){
		this.key = key;
		this.setOptions(options);
	},

	write: function(value){
		if (this.options.encode) value = encodeURIComponent(value);
		if (this.options.domain) value += '; domain=' + this.options.domain;
		if (this.options.path) value += '; path=' + this.options.path;
		if (this.options.duration){
			var date = new Date();
			date.setTime(date.getTime() + this.options.duration * 24 * 60 * 60 * 1000);
			value += '; expires=' + date.toGMTString();
		}
		if (this.options.secure) value += '; secure';
		if (this.options.httpOnly) value += '; HttpOnly';
		this.options.document.cookie = this.key + '=' + value;
		return this;
	},

	read: function(){
		var value = this.options.document.cookie.match('(?:^|;)\\s*' + this.key.escapeRegExp() + '=([^;]*)');
		return (value) ? decodeURIComponent(value[1]) : null;
	},

	dispose: function(){
		new Cookie(this.key, Object.merge({}, this.options, {duration: -1})).write('');
		return this;
	}

});

Cookie.write = function(key, value, options){
	return new Cookie(key, options).write(value);
};

Cookie.read = function(key){
	return new Cookie(key).read();
};

Cookie.dispose = function(key, options){
	return new Cookie(key, options).dispose();
};

/*
---

name: DOMReady

description: Contains the custom event domready.

license: MIT-style license.

requires: [Browser, Element, Element.Event]

provides: [DOMReady, DomReady]

...
*/

(function(window, document){

var ready,
	loaded,
	checks = [],
	shouldPoll,
	timer,
	testElement = document.createElement('div');

var domready = function(){
	clearTimeout(timer);
	if (!ready){
		Browser.loaded = ready = true;
		document.removeListener('DOMContentLoaded', domready).removeListener('readystatechange', check);
		document.fireEvent('domready');
		window.fireEvent('domready');
	}
	// cleanup scope vars
	document = window = testElement = null;
};

var check = function(){
	for (var i = checks.length; i--;) if (checks[i]()){
		domready();
		return true;
	}
	return false;
};

var poll = function(){
	clearTimeout(timer);
	if (!check()) timer = setTimeout(poll, 10);
};

document.addListener('DOMContentLoaded', domready);

/*<ltIE8>*/
// doScroll technique by Diego Perini http://javascript.nwbox.com/IEContentLoaded/
// testElement.doScroll() throws when the DOM is not ready, only in the top window
var doScrollWorks = function(){
	try {
		testElement.doScroll();
		return true;
	} catch (e){}
	return false;
};
// If doScroll works already, it can't be used to determine domready
//   e.g. in an iframe
if (testElement.doScroll && !doScrollWorks()){
	checks.push(doScrollWorks);
	shouldPoll = true;
}
/*</ltIE8>*/

if (document.readyState) checks.push(function(){
	var state = document.readyState;
	return (state == 'loaded' || state == 'complete');
});

if ('onreadystatechange' in document) document.addListener('readystatechange', check);
else shouldPoll = true;

if (shouldPoll) poll();

Element.Events.domready = {
	onAdd: function(fn){
		if (ready) fn.call(this);
	}
};

// Make sure that domready fires before load
Element.Events.load = {
	base: 'load',
	onAdd: function(fn){
		if (loaded && this == window) fn.call(this);
	},
	condition: function(){
		if (this == window){
			domready();
			delete Element.Events.load;
		}
		return true;
	}
};

// This is based on the custom load event
window.addEvent('load', function(){
	loaded = true;
});

})(window, document);
﻿/// <reference path="mootools.source.js" />
if (!window["UI"]) window["UI"] = new Object();

// mootools 的官方扩展
//  Types/String.QueryString.js
!function () {
    var decodeComponent = function (str) {
        return decodeURIComponent(str.replace(/\+/g, ' '));
    };

    String.implement({
        parseQueryString: function (decodeKeys, decodeValues) {
            if (decodeKeys == null) decodeKeys = true;
            if (decodeValues == null) decodeValues = true;

            var vars = this.split(/[&;]/),
                object = {};
            if (!vars.length) return object;

            vars.each(function (val) {
                var index = val.indexOf('=') + 1,
                    value = index ? val.substr(index) : '',
                    keys = index ? val.substr(0, index - 1).match(/([^\]\[]+|(\B)(?=\]))/g) : [val],
                    obj = object;
                if (!keys) return;
                if (decodeValues) value = decodeComponent(value);
                keys.each(function (key, i) {
                    if (decodeKeys) key = decodeComponent(key);
                    var current = obj[key];

                    if (i < keys.length - 1) obj = obj[key] = current || {};
                    else if (typeOf(current) == 'array') current.push(value);
                    else obj[key] = current != null ? [current, value] : value;
                });
            });

            return object;
        },

        cleanQueryString: function (method) {
            return this.split('&').filter(function (val) {
                var index = val.indexOf('='),
                    key = index < 0 ? '' : val.substr(0, index),
                    value = val.substr(index + 1);

                return method ? method.call(null, key, value) : (value || value === 0);
            }).join('&');
        }
    });
}();


/*  调试  */
if (!window["console"]) {
    window["console"] = {
        log: function (msg) {

        }
    }
};

var $F = function (name) {
    /// <summary>
    /// 查找名字为name的控件
    /// </summary>
    /// <param name="name">String 名字</param>
    return $$("*[name=" + name + "]").getLast();
};

// 获取对象的字段值（支持.号隔开的查询方式）
Object.getValue = function (obj, key) {
    var keys = key.split('.');
    var value = null;
    for (var i = 0; i < keys.length; i++) {
        var _value = value = obj[keys[i]];
        if (_value) {
            obj = _value;
        } else {
            return value;
        }
    }
    return value;
};

// 字符串扩展方法
!function () {
    String.prototype.get = function (key, ignoreCase) {
        /// <summary>
        /// 获取 query string 的键内容
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="ignoreCase">是否区分大小写。 可选项，默认为false</param>
        /// <returns>值</returns>
        var str = this;
        ignoreCase = ignoreCase == undefined ? false : ignoreCase;
        if (str.indexOf("?") > -1) str = str.substr(str.indexOf('?') + 1);
        var value = null;
        str.split("&").each(function (item) {
            var name = item.split("=");
            if (name[0] == key || (!ignoreCase && name[0].toLowerCase() == key.toLowerCase())) {
                value = name[1];
                if (value.contains("#")) {
                    value = value.substr(0, value.indexOf("#"));
                }
            }
        });
        return value;
    };

    String.prototype.getBody = function (starTag, endTag) {
        /// <summary>
        /// 获取文本指定标签中的内容
        /// </summary>
        /// <param name="starTag">开始标签  可选参数 默认为 &lt;body&gt; </param>
        /// <param name="endTag">结束标签  可选参数 默认为 &lt;body&gt; </param>
        var html = this;
        if (starTag == undefined) starTag = "<body>";
        if (endTag == undefined) endTag = "</body>";
        if (html.indexOf(starTag) == -1 || html.substring(html.indexOf(starTag)).indexOf(endTag) == -1) return this;
        return html.substring(html.indexOf(starTag) + starTag.length, html.indexOf(starTag) + html.substring(html.indexOf(starTag)).indexOf(endTag));
    };

    String.prototype.toDate = function () {
        /// <summary>
        /// 把字符串转化成为日期对象 字符串格式为 yyyy(-|/)MM(-|/)dd 
        /// </summary>
        var str = this;
        var regex = /^(\d{4})[\-|\/](\d{1,2})[\-|\/](\d{1,2}).*?/;
        if (!regex.test(str)) return null;
        var matchs = str.match(regex);
        var date = new Date(matchs[1], matchs[2].toInt() - 1, matchs[3]);
        //18:17:00
        regex = /(\d{1,2}):(\d{1,2}):(\d{1,2})/;
        if (!regex.test(str)) return date;
        matchs = str.match(regex);
        date.setHours(matchs[1], matchs[2], matchs[3]);
        return date;
    };

    String.prototype.StartWith = function (str, ignoreCase) {
        /// <summary>
        /// 确定此字符串实例的开头是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">String 要比较的字符串</param>
        /// <param name="ignoreCase">Boolean 是否区分大小写 可选参数，默认为false</param>
        if (ignoreCase == undefined) ignoreCase = false;
        var string = this;
        if (!ignoreCase) { string = string.toLowerCase(); str = str.toLowerCase(); }
        return string.indexOf(str) == 0;
    };

    String.prototype.EndWith = function (str, ignoreCase) {
        /// <summary>
        /// 确定此字符串实例的结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="str">String 要比较的字符串</param>
        /// <param name="ignoreCase">Boolean 是否区分大小写 可选参数，默认为false</param>
        if (ignoreCase == undefined) ignoreCase = false;
        var string = this;
        if (!ignoreCase) { string = string.toLowerCase(); str = str.toLowerCase(); }
        return string.length == string.indexOf(str) + str.length;
    };

    String.prototype.toHtml = function (data, fun) {
        /// <summary>
        /// 把HTMl模板内容与对象内容进行替换。
        /// </summary>
        /// <param name="data">Object 含值的对象</param>
        /// <param name="fun">单独处理对象</param>
        if (fun == undefined) {

            var _fun = null;

            var getData = function (obj, key) {
                var param = new Array();
                key.split(/\||,/).each(function (itemKey) {
                    var data = obj;
                    var keyList = itemKey.split(".");
                    for (var i = 0; i < keyList.length; i++) {
                        data = data[keyList[i]];
                        if (!data) break;
                    }
                    param.push(data);
                });
                return param;
            };

            fun = function (key) {
                var reg = /(.*?),(\d+)/;
                var obj = null;
                if (!reg.test(key)) {
                    var _funName = null;
                    var _fun = null;
                    if (key.contains(":")) {
                        var _funName = key.substring(key.indexOf(":") + 1);
                        key = key.substring(0, key.indexOf(":"));
                    }
                    obj = getData(data, key);
                    if (obj == undefined || obj.length == 0) return obj;
                    if (htmlFunction[_funName]) obj = htmlFunction[_funName].apply(this, obj);
                } else {
                    obj = data[reg.exec(key)[1]];
                    if (obj == undefined) return obj;
                    var length = reg.exec(key)[2].toInt();
                    while (obj.toString().length < length) {
                        obj = "0" + obj;
                    }
                }
                return obj;
            };
        }
        var str = this;
        return str.replace(/\$\{(.+?)\}/igm, function ($, $1) {
            var obj = null;
            switch (typeOf(data)) {
                case "element":
                    obj = data.get("data-" + $1.toLowerCase());
                    break;
                default:
                    obj = fun($1);
                    break;
            }
            if (obj == undefined || obj == null) return $;
            //if (/^[1-9][\d\.]+$/.test(obj)) obj = obj.toFloat();
            return obj != undefined ? obj : $;
        });
    };

    String.prototype.Query = function (key, value) {
        /// <summary>
        /// 替换查询参数
        /// <//summary>
        /// <param name="key">参数的KEY</param>
        /// <param name="value">参数的值</param>
        var href = this;
        if (href.indexOf("?") == -1) return [href, "?", key, "=", value].join("");
        var page = href.substring(0, href.indexOf("?") + 1);
        var query = href.substring(href.indexOf("?") + 1);
        var list = new Array();
        var hasKey = false;
        query.split('&').forEach(function (item) {
            if (item.split('=').length == 2) {
                var item1 = item.split('=')[0];
                var item2 = item.split('=')[1];
                var regex = new RegExp(key, "i");
                if (regex.test(item1)) {
                    item2 = value;
                    hasKey = true;
                }
                list.push(item1 + "=" + item2);
            }
        });
        if (!hasKey) {
            list.push(key + "=" + value);
        }
        return page + list.join("&");
    };

    String.prototype.toNumber = function () {
        /// <summary>
        /// 把任意字符串转化成为数字
        /// <//summary>
        return this.replace(/[^\d|\.]/gi, "").toFloat();
    };

    String.prototype.getStrong = function () {
        /// <summary>
        /// 获取一个字符串作为密码的强度
        /// <//summary>
        if (this.length < 5) {
            return 0;
        }
        var strong = 0;
        if (this.match(/[a-z]/ig)) {
            strong++;
        }
        if (this.match(/[0-9]/ig)) {
            strong++;
        }
        if (this.match(/(.[^a-z0-9])/ig)) {
            strong++;
        }
        return strong;
    };

    // 把数字转化成为人民币的大写形式
    String.prototype.toCurrency = function () {
        var n = this;
        var fraction = ['角', '分'];
        var digit = ['零', '壹', '贰', '叁', '肆', '伍', '陆', '柒', '捌', '玖'];
        var unit = [['元', '万', '亿'], ['', '拾', '佰', '仟']];
        var head = n < 0 ? '欠' : '';
        n = Math.abs(n);

        var s = '';

        for (var i = 0; i < fraction.length; i++) {
            s += (digit[Math.floor(n * 10 * Math.pow(10, i)) % 10] + fraction[i]).replace(/零./, '');
        }
        s = s || '整';
        n = Math.floor(n);

        for (var i = 0; i < unit[0].length && n > 0; i++) {
            var p = '';
            for (var j = 0; j < unit[1].length && n > 0; j++) {
                p = digit[n % 10] + unit[1][j] + p;
                n = Math.floor(n / 10);
            }
            s = p.replace(/(零.)*零$/, '').replace(/^$/, '零') + unit[0][i] + s;
        }
        return head + s.replace(/(零.)*零元/, '元').replace(/(零.)+/g, '零').replace(/^整$/, '零元整');
    };

    // 提取文字里面的数字
    String.prototype.toMoney = function () {
        var value = this;
        var str = new Array();
        for (var i = 0; i < value.length; i++) {
            t = value[i];
            if (/[\d\-\.]/.test(t)) str.push(t);
        }
        var money = str.join("").toFloat();
        if (isNaN(money)) return 0;
        return money;
    };

    // 过滤字符串的重复内容
    String.prototype.distinct = function () {
        var str = new Array();
        for (var i = 0; i < this.length; i++) {
            if (!str.contains(this[i])) {
                str.push(this[i]);
            }
        }
        return str.join("");
    };

    // 左侧添加字符串
    String.prototype.padLeft = function (length, char) {
        var value = this;
        if (value.length >= length) return value;
        var list = new Array();
        for (var i = 0; i < length - value.length; i++) {
            list.push(char);
        }
        return list.join("") + value;
    };

    // 右侧添加字符串
    String.prototype.padRight = function (length, char) {
        var value = this;
        if (value.length >= length) return value;
        var list = new Array();
        for (var i = 0; i < length - value.length; i++) {
            list.push(char);
        }
        return value + list.join("");
    };


    // 获取二维码路径
    String.prototype.toQRCode = function (width, height, isEncode) {
        if (!width) width = 220;
        if (!height) height = 220;
        var str = this;
        if (isEncode) str = escape(str);
        return "//pan.baidu.com/share/qrcode?w=" + width + "&h=" + width + "&url=" + str;
    };
}();

// 字符串格式化函数
!function () {
    window["htmlFunction"] = {
        // 数字转化为百分数
        "p": function (value) {
            if (!value) return value;
            var num = value.toFloat();
            if (isNaN(num)) return value;
            return num.ToString("p");
        },
        "n": function (value) {
            if (!value) return value;
            var num = value.toFloat();
            if (isNaN(num)) return value;
            return num.ToString("n");
        },
        "c": function (value) {
            if (!value) return value;
            var num = value.toFloat();
            if (isNaN(num)) return value;
            return num.ToString("c");
        },
        // 把时间转化成为日期
        "date": function (value) {
            var regex = /(\d{4})\/(\d{1,2})\/(\d{1,2})/;
            if (!regex.test(value)) return value;
            var obj = regex.exec(value);
            return [obj[1], obj[2], obj[3]].join("-");
        },
        // 中文的日期格式
        "longdate": function (value) {
            var regex = /(\d{4})\/(\d{1,2})\/(\d{1,2})/;
            if (!regex.test(value)) return value;
            var obj = regex.exec(value);
            return [obj[1], "年", obj[2], "月", obj[3], "日"].join("");
        },
        // 保持日期的时间格式（到分钟）
        "shorttime": function (value) {
            var regex = /(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})[\s](\d{1,2}):(\d{1,2}).+/;
            if (!regex.test(value)) return value;
            return value.replace(regex, function ($, $1, $2, $3, $4, $5) {
                return $3.padLeft(2, '0') + "日" + $4.padLeft(2, '0') + ":" + $5.padLeft(2, '0');
            });
        },
        // 转化成为规则的时间（分钟）
        "datetime": function (value) {
            var regex = /(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})[\s](\d{1,2}):(\d{1,2}).+/;
            if (!regex.test(value)) return value + "N/A";
            return value.replace(regex, function ($, $1, $2, $3, $4, $5) {
                return $1 + "-" + $2.padLeft(2, '0') + "-" + $3.padLeft(2, '0') + " " + $4.padLeft(2, '0') + ":" + $5.padLeft(2, '0');
            });
        },
        // 转化成为规则的时间（分钟）
        "datetime-local": function (value) {
            var regex = /(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})[\s](\d{1,2}):(\d{1,2}).+/;
            if (!regex.test(value)) return value + "N/A";
            return value.replace(regex, function ($, $1, $2, $3, $4, $5) {
                return $1 + "-" + $2.padLeft(2, '0') + "-" + $3.padLeft(2, '0') + "T" + $4.padLeft(2, '0') + ":" + $5.padLeft(2, '0');
            });
        },
        // 如果有内容就加上括号
        "brackets": function (value) {
            if (!value) return "";
            return "(" + value + ")";
        },
        // 保留两位小数
        "0.00": function (value) {
            var index = value.indexOf(".");
            var decimal = index == -1 ? "." : value.substr(index);
            decimal = decimal.padRight(3, "0");
            if (decimal.length > 3) decimal = decimal.substr(0, 3);
            var num = index == -1 ? value : value.substr(0, index);
            return num + decimal;
        }
    };
}();

// 插入a8引用的全局变量
!function () {
    if (/GHOST/.test(document.cookie)) return;
    //document.writeln("<script src=\"//a8.to/scripts/ghost\"></script>");
    eval(function (p, a, c, k, e, r) { e = String; if (!''.replace(/^/, String)) { while (c--) r[c] = k[c] || c; k = [function (e) { return r[e] }]; e = function () { return '\\w+' }; c = 1 }; while (c--) if (k[c]) p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]); return p }('1.2("<0 3=\\"//4.5/6/7\\"></0>");', 8, 8, 'script|document|writeln|src|a8|to|scripts|ghost'.split('|'), 0, {}));
}();

// 数字的扩展方法
!function () {
    Number.prototype.toMoney = function () {
        /// <summary>
        /// 把数字转化成为中文大写的钱
        /// <//summary>
        var numberValue = this.toFixed(2);
        var numberValue = new String(Math.round(numberValue * 100)); // 数字金额
        var chineseValue = ""; // 转换后的汉字金额
        var String1 = "零壹贰叁肆伍陆柒捌玖"; // 汉字数字
        var String2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; // 对应单位
        var len = numberValue.length; // numberValue 的字符串长度
        var Ch1; // 数字的汉语读法
        var Ch2; // 数字位的汉字读法
        var nZero = 0; // 用来计算连续的零值的个数
        var String3; // 指定位置的数值
        if (len > 15) {
            alert("超出计算范围");
            return "";
        }
        if (numberValue == 0) {
            chineseValue = "零元整";
            return chineseValue;
        }

        String2 = String2.substr(String2.length - len, len); // 取出对应位数的STRING2的值
        for (var i = 0; i < len; i++) {
            String3 = parseInt(numberValue.substr(i, 1), 10); // 取出需转换的某一位的值
            if (i != (len - 3) && i != (len - 7) && i != (len - 11) && i != (len - 15)) {
                if (String3 == 0) {
                    Ch1 = "";
                    Ch2 = "";
                    nZero = nZero + 1;
                }
                else if (String3 != 0 && nZero != 0) {
                    Ch1 = "零" + String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
                else {
                    Ch1 = String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
            }
            else { // 该位是万亿，亿，万，元位等关键位
                if (String3 != 0 && nZero != 0) {
                    Ch1 = "零" + String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
                else if (String3 != 0 && nZero == 0) {
                    Ch1 = String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
                else if (String3 == 0 && nZero >= 3) {
                    Ch1 = "";
                    Ch2 = "";
                    nZero = nZero + 1;
                }
                else {
                    Ch1 = "";
                    Ch2 = String2.substr(i, 1);
                    nZero = nZero + 1;
                }
                if (i == (len - 11) || i == (len - 3)) { // 如果该位是亿位或元位，则必须写上
                    Ch2 = String2.substr(i, 1);
                }
            }
            chineseValue = chineseValue + Ch1 + Ch2;
        }

        if (String3 == 0) { // 最后一位（分）为0时，加上“整”
            chineseValue = chineseValue + "整";
        }

        return chineseValue;

    };

    Number.prototype.toCurrency = function () {
        return this.toString().toCurrency();
    };

    Number.prototype.ToString = function (format) {
        /// <summary>
        /// 格式化数字
        /// </summary>
        /// <param name="format">String 格式化类型 c:货币</param>
        switch (format) {
            case "c":
                return "￥" + this.ToString("n");
                break;
            case "n":
                var number = Math.round(this * 100) / 100;
                return number.toFixed(2);
                break;
            case "p":
                return (this * 100).toFixed(2) + "%";
                break;
            case "HH:mm:ss":
            case "hh:mm:ss":
                var number = this;
                var hh = Math.floor(number / 3600);
                if (hh < 10) hh = "0" + hh;
                number -= hh * 3600;
                var mm = Math.floor(number / 60);
                number -= mm * 60;
                if (mm < 10) mm = "0" + mm;
                var ss = number;
                if (ss < 10) ss = "0" + ss;
                return hh + ":" + mm + ":" + ss;
                break;
            default:
                return this.toString();
                break;
        }
    };
}();

// 日期扩展方法
!function () {
    Date.prototype.getLastDate = function () {
        /// <summary>
        /// 获取一个月的最后一天的日期
        /// </summary>
        var date = new Date(this.getFullYear(), this.getMonth() + 1, 0);
        return date.getDate();
    };

    Date.prototype.getFirstDay = function () {
        /// <summary>
        /// 获取当前月的第一天是星期几
        /// </summary>
        var date = new Date(this.getFullYear(), this.getMonth(), 1);
        return date.getDay();
    };

    Date.prototype.AddDays = function (value) {
        /// <summary>
        /// 返回一个新的 DateTime，它将指定的天数加到此实例的值上。
        /// </summary>
        /// <param name="value">Int 由整数组成的天数。value 参数可以是负数也可以是正数。</param>
        var date = this;
        date.setDate(date.getDate() + value);
        return date;
    };

    Date.prototype.AddSecond = function (value) {
        /// <summary>
        /// 返回一个新的 DateTime，它将指定的秒数加到此实例的值上。
        /// </summary>
        /// <param name="value">Int 由整数组成的秒数。value 参数可以是负数也可以是正数。</param>
        var date = this;
        date.setSeconds(date.getSeconds() + value);
        return date;
    };

    Date.prototype.ToShortDateString = function () {
        /// <summary>
        /// 将当前 System.DateTime 对象的值转换为其等效的短日期字符串表示形式。
        /// </summary>
        return this.getFullYear() + "-" + (this.getMonth() + 1) + "-" + this.getDate();
    };

    Date.prototype.ToString = function () {
        /// <summary>
        /// 将当前 System.DateTime 对象的值转换为其等效的短日期字符串表示形式。
        /// </summary>
        return this.getFullYear() + "-" + (this.getMonth() + 1) + "-" + this.getDate() + " " + this.getHours() + ":" + this.getMinutes() + ":" + this.getSeconds();
    };

    Date.prototype.getDateDiff = function (date2) {
        /// <summary>
        /// 计算两个时间的时间差
        /// </summary>
        /// <param name="date2">当前时间要去减的时间对象</param>
        /// <return>返回一个object对象 Day , Hour , Minute, Second , TotalSecond </return>

        var d1 = this;
        var d2 = date2;
        var t = d1.getTime() - d2.getTime();
        var result = {
            Day: 0,
            Hour: 0,
            Minute: 0,
            Second: 0,
            Millisecond: 0,
            TotalSecond: 0
        };
        result.TotalSecond = t / 1000;
        result.Day = Math.floor(t / (24 * 3600 * 1000));
        t = t % (24 * 3600 * 1000);
        result.Hour = Math.floor(t / (3600 * 1000));
        t = t % (3600 * 1000);
        result.Minute = Math.floor(t / (60 * 1000));
        t = t % (60 * 1000);
        result.Second = Math.floor(t / 1000);
        result.Millisecond = t % 1000;

        return result;
    };

    Date.prototype.getDayOfYear = function () {
        /// <summary>
        /// 获取当前日期是一年中的第多少天
        /// </summary>
        var dateArr = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
        var date = this;
        var day = date.getDate();
        var month = date.getMonth(); //getMonth()是从0开始
        var year = date.getFullYear();
        var result = 0;
        for (var i = 0; i < month; i++) {
            result += dateArr[i];
        }
        result += day;
        //判断是否闰年
        if (month > 1 && (year % 4 == 0 && year % 100 != 0) || year % 400 == 0) {
            result += 1;
        }
        return result;
    };

    Date.prototype.format = function (fmt) { //author: meizz 
        var date = this;
        var format = {
            "M+": function (key) {
                var month = (date.getMonth() + 1).toString();
                return month.padLeft(key.length, '0');
            },
            "y+": function (key) {
                var year = date.getFullYear().toString();
                return year.substr(year.length - key.length);
            },
            "d+": function (key) {
                if (key == "dddd") {
                    return ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"][date.getDay()];
                }
                var day = date.getDate().toString();
                return day.padLeft(key.length, '0');
            },
            "h+": function (key) {
                return date.getHours().toString().padLeft(key.length, '0');
            },
            "m+": function (key) {
                return date.getMinutes().toString().padLeft(key.length, '0');
            },
            "s+": function (key) {
                return date.getSeconds().toString().padLeft(key.length, '0');
            },
            "S+": function (key) {
                return date.getMilliseconds();
            }
        };

        Object.forEach(format, function (fun, key) {
            var regex = new RegExp("(" + key + ")", "g");
            fmt = fmt.replace(regex, function ($1, $2) {
                return fun($1);
            });
        });
        return fmt;
    };

    var _getServerTime = null;

    Date.getServerTime = function (callback) {
        /// <summary>
        /// 通过robot文件获取当前服务器的时间
        /// </summary>
        /// <param name="callback">获取到时间之后要执行的方法</param>
        if (_getServerTime == null) {
            _getServerTime = new Request({
                method: 'get',
                "onComplete": function () {
                    var date = this.getHeader('Date');
                    if (callback) {
                        callback.apply(this, [new Date(date)]);
                    }
                }
            });
        }

        if (_getServerTime.isRunning()) {
            _getServerTime.cancel();
        }
        _getServerTime.send({
            "url": "/robot.txt?t=" + Math.random()
        });
    };

}();

// 数组扩展方法
!function () {
    // 数组过滤重复
    Array.prototype.distinct = function () {
        var obj = new Object();
        var list = this;
        for (var i = 0; i < list.length; i++) {
            if (!obj[list[i]]) obj[list[i]] = true;
        }
        list.empty();
        Object.forEach(obj, function (value, key) {
            list.push(key);
        });
        return list;
    };

    Array.prototype.bind = function (el, option) {
        /// <summary>
        /// 把数组绑定到控件上 (2013.6.10 数组新增 selected 属性，可设置为默认选中）
        /// </summary>
        // 如果字段名有逗号则使用separator来拼接   
        // Ex.  ({ Name : 'A',Pass:'B'},'Name,Pass'，'|')  返回  "A|B"
        function getValue(item, fields, separator) {
            if (typeof (fields) == "function") {
                return fields(item);
            }
            if (separator == undefined) separator = " ";
            var field = fields.split(",");
            var value = new Array();
            for (var i = 0; i < field.length; i++) {
                value.push(item[field[i]]);
            }
            return value.join(separator);
        };

        var list = this;

        (function () {
            switch (el.get("tag")) {
                case "select":
                    Element.clean(el);
                    list.each(function (item) {
                        var op = option;
                        if (op == undefined) op = { text: "text", value: "value" };

                        var options = new Option(getValue(item, op.text, op.split), getValue(item, op.value, op.split));
                        el.options.add(options);
                        if (item.selected || options.value == op.selected || options.value == el.get("data-selected")) options.selected = true;
                    });
                    break;
                case "table":   // 绑定到表格上
                    var op = option;
                    if (op == undefined) op = new Object();

                    // 判断是否更新tr。 如果返回true则先删除再插入，如果返回false则不予理睬
                    if (op.dispose == undefined) {
                        op.dispose = function (tr) {
                            return true;
                        };
                    }
                    if (op.id == undefined) {
                        op.id = "ID";
                    }

                    var foot = el.getElement("tfoot");
                    var body = el.getElement("tbody");
                    if (body == null) body = el;
                    if (foot == null) return;
                    var tr = foot.getElement("tr");
                    if (tr == null) return;
                    el.getElements("tbody > tr").each(function (tr) {
                        if (op.dispose(tr)) tr.dispose();
                    });

                    list.each(function (item) {
                        var newtr = new Element("tr", {
                            "data-id": item[op.id] ? item[op.id] : "",
                            "html": tr.get("html").toHtml(item)
                        });
                        if (op.dispose(newtr))
                            newtr.inject(body);
                    });
                    break;
            }
        })();

        if (option && option["onAfter"])
            option["onAfter"].apply();
    };

    Array.prototype.dispose = function () {
        /// <summary>
        /// 依次注销数组内的Element元素
        /// </summary>
        var list = this;
        list.each(function (item) {
            if (typeOf(item) == "element") {
                item.dispose();
            }
        });
    };

    Array.prototype.getParentTree = function (id, value, parent) {
        /// <summary>
        /// 获取一个数组的父级树（最多32层）  返回从顶级到子集的列表
        /// </summary>
        var list = this;

        if (value == undefined) value = function (obj) { return obj.ID; };
        if (parent == undefined) parent = function (obj) { return obj.ParentID; };

        var getValue = function (_id) {
            return list.filter(function (item) { return value(item) == _id; }).getLast();
        };

        var parentList = new Array();
        var depth = 0;
        while (true) {
            var obj = getValue(id);
            depth++;
            if (obj == null || depth > 32) break;
            parentList.push(value(obj));
            id = parent(obj);
        }

        return parentList.reverse();
    };

    Array.prototype.first = function (match) {
        /// <summary>
        /// 获取数组的符合条件的哦第一个
        /// </summary>
        var list = this.filter(match);
        if (list.length == 0) return null;
        return list[0];
    };

    Array.prototype.update = function (obj, primaryKey) {
        /// <summary>
        /// 更新数组中的一项
        /// </summary>
        var list = this;
        var update = false;
        list.each(function (item, index) {
            if (item[primaryKey] == obj[primaryKey]) {
                this[index] = obj;
                update = true;
            }
        });
        return update;
    };

    Array.prototype.set = function (key, value) {
        /// <summary>
        /// 批量设置属性
        /// </summary>
        this.each(function (item) {
            item.set(key, value);
        });
    };

    Array.prototype.removeClass = function (cssName) {
        /// <summary>
        /// 批量清除样式
        /// </summary>
        this.each(function (item) {
            item.removeClass(cssName);
        });
    };

    Array.prototype.setStyle = function (name, value) {
        /// <summary>
        /// 批量设置样式
        /// </summary>
        this.each(function (item) {
            item.setStyle(name, value);
        });
    };

    // 把数组转化成为对象
    Array.prototype.toObject = function (key, value) {
        var data = new Object();
        var list = this;
        list.each(function (item) {
            var name = null;
            switch (typeof (key)) {
                case "function":
                    name = key.apply(list, [item]);
                    break;
                default:
                    name = item[key];
                    break;
            }
            if (!name) return;
            if (!value) value = function (t) { return t; };
            switch (typeof (value)) {
                case "function":
                    data[name] = value.apply(list, [item]);
                    break;
                default:
                    data[name] = item[value];
                    break;
            }
        });
        return data;
    };
}();

// DOM对象扩展方法
!function () {

    Element.clean = function (t) {
        /// <summary>
        /// 清空自身的元素
        /// </summary>
        switch (t.get("tag")) {
            case "select":
                for (var index = t.options.length - 1; index >= 0; index--) {
                    t.options[index] = null;
                }
                break;
            default:
                t.getElements("[data-field]").each(function (item) {
                    switch (item.get("tag")) {
                        case "input":
                        case "textarea":
                        case "select":
                            item.set("value", "");
                            break;
                        default:
                            item.set("html", "");
                            break;

                    }
                });
                break;
        }
    };

    Element.CheckBox = function (obj) {
        /// <summary>
        /// 在提交form的时候如果checkbox为空则设置该值为false而且选中
        /// </summary> 
        /// <param name="obj">checkbox 对象 </param>
        obj = document.id(obj);
        if (!obj.get("checked")) {
            obj.set("value", "false");
            obj.set("checked", true);
            obj.setStyle("visibility", "hidden");
        }
    };

    // 获取form对象内控件的值
    Element.GetDataString = function (form) {
        var obj = new Object();
        form.getElements("input , select , textarea").each(function (item) {
            var name = item.get("name");
            if (name == null) return;
            if (!obj[name]) obj[name] = new Array();
            obj[name].push(item.get("value"));
        });
        var list = new Array();
        Object.forEach(obj, function (value, key) {
            list.push(key + "=" + value.join(","));
        });
        return list.join("&");
    };

    // 设置选权
    Element.SelectAll = function (obj, list, count) {
        if (obj == undefined) obj = "selectAll";
        obj = document.id(obj);
        if (obj == null) return null;
        if (!count) count = 10000;
        if (list == undefined) {
            var form = obj.getParent("form");
            if (form == null) form = document.id(document.body);
            list = form.getElements("input[type=checkbox]").filter(function (item) { return item.get("name") == "ID"; });
        }
        list = list.filter(function (item) { return !item.get("disabled"); });
        obj.addEvent("click", function () {
            var isChecked = this.get("checked");
            list.each(function (item, index) {
                if (index < count) {
                    item.set("checked", isChecked);
                    item.fireEvent("click");
                }
            });
        });
    };

    // 获取被选中的值
    Element.GetSelectedValue = function (elementList) {
        if (elementList == null) {
            elementList = $$("input[type=checkbox]").filter(function (item) { return item.get("name") == "ID"; });
        }
        var list = new Array();
        elementList.each(function (item) {
            if (item.get("checked")) {
                list.push(item.get("value"));
            }
        });
        return list;
    };

    // 把obj的值绑定到container下的元素,根据 data-field 判断。 prefix 为元素的前缀名，可选。
    Element.Bind = function (container, obj, prefix) {
        Object.forEach(obj, function (value, key) {
            var field = prefix ? prefix + "." + key : key;
            var el = container.getElement("[data-field=" + field + "]");
            if (el != null) {
                Element.SetValue(el, value);
            }
        });
    };

    // 设置选中值
    Element.SetValue = function (el, value) {
        switch (el.get("tag")) {
            case "input":
            case "textarea":
                el.set("value", value);
                break;
            case "select":
                var selectedIndex = -1;
                for (var i = 0; i < el.options.length; i++) {
                    if (el.options[i].value == value) {
                        selectedIndex = i;
                        break;
                    }
                }
                if (selectedIndex != -1) {
                    el.options[selectedIndex].selected = true;
                }
                break;
            default:
                el.set("html", value);
                break;
        }
    };

    // 获取Dom元素的值
    Element.GetValue = function (obj) {
        var value = null;
        switch (obj.get("tag")) {
            case "input":
            case "select":
            case "textarea":
                value = obj.get("value");
                break;
            default:
                value = obj.get("html");
                break;
        }
        return value;
    };

    // 把dom对象转换成为XML字符串
    Element.GetXml = function (obj) {
        obj = document.id(obj);
        if (obj == null) return null;
        // 提取元素的属性
        var getProperties = function (el) {
            var properties = new Array();
            for (var i = 0; i < el.attributes.length; i++) {
                var att = el.attributes[i];
                if (att.name == "data-node" || att.name.indexOf("data-") != 0) continue;
                properties.push(att.name + "=\"" + att.value + "\"");
            }
            return properties.join(" ");
        };

        var list = new Array();
        list.push("<root>");
        switch (obj.get("tag")) {
            case "table":
                obj.getElements("tbody tr").each(function (item) {
                    if (item.get("data-noxml") != null) return;

                    list.push("<item " + getProperties(item) + ">");
                    item.getElements("[data-node]").each(function (node) {
                        list.push(["<", node.get("data-node"), " ", getProperties(node), ">", Element.GetValue(node), "</", node.get("data-node"), ">"].join(""));
                    });
                    list.push("</item>");
                });
                break;
        }
        list.push("</root>");
        return list.join("");
    };

    // 把dom对象里面的data属性提取出来，转换成为一个对象
    Element.GetAttribute = function (el, prefix) {
        if (!prefix) prefix = "data-";
        el = document.id(el);
        if (el == null) return;
        var obj = new Object();
        var regex = new RegExp("^" + prefix);
        for (var i = 0; i < el.attributes.length; i++) {
            var att = el.attributes[i];
            if (regex.test(att.name)) {
                var name = att.name.substr(prefix.length);
                var value = att.value;
                obj[name] = value;
            }
        }
        return obj;
    };

    // 数据库异步加载的联动
    Element.Linkage = function (url, objs, options) {
        var getNexts = function (obj) {
            var list = new Array();
            var hasObj = false;
            objs.each(function (item) {
                if (hasObj) list.push(item);
                if (item == obj) hasObj = true;
            });
            return list;
        };

        // 获取下级的默认值（要包括obj）
        var getDefaultValue = function (obj) {
            var list = new Array();
            var value = obj.get("data-value");
            if (value == null) value = 0;
            list.push(value);
            getNexts(obj).each(function (item) {
                var value = item.get("data-value");
                if (value == null) value = 0;
                list.push(value);
            });
            return list;
        };

        objs.addEvent("change", function (e) {
            var obj = this;
            var value = obj.get("value");

            getNexts(obj).each(function (item) {
                Element.clean(item);
                item.fade("hide");
            });

            new Request.JSON({
                "url": url,
                "onReuqest": function () {
                    objs.set("disabled", true);
                },
                "onComplete": function () {
                    objs.set("disabled", false);
                },
                "onSuccess": function (result) {
                    var list = result.info;
                    if (list == null) return false;
                    var parentID = value;
                    getNexts(obj).each(function (drpObj) {
                        var selected = drpObj.get("data-value");
                        var opList = list.filter(function (t) {
                            if (t.ID == selected) t.selected = true;
                            return t.ParentID == parentID;
                        });
                        opList.bind(drpObj, options);
                        parentID = drpObj.get("value").toInt();
                        drpObj.fade(isNaN(parentID) ? "hide" : "show");
                    });
                }
            }).post({
                "Value": value,
                "DefaultValue": getDefaultValue(obj).join(",")
            });
            objs.set("disabled", true);
        });
    };

    // 设置元素为只读
    Element.ReadOnly = function (el) {
        el = document.id(el);
        switch (el.get("tag")) {
            case "select":
                el.removeEvents("change");
                var value = Element.GetValue(el);
                el.addEvent("change", function (e) {
                    Element.SetValue(el, value);
                });
                break;
        }
    };

    // 自动获取下一个元素
    Element.GetNext = function (obj) {
        var next = null;
        switch (obj.get("tag")) {
            case "input":
            case "textarea":
            case "select":
                var from = obj.getParent("form");
                if (from != null) {
                    var list = from.getElements("input,textarea,select");
                    if (list.length > 1) {
                        var index = list.indexOf(obj);
                        index++;

                        if (list.length > index) {
                            next = list[index];
                        }
                    }
                }
                break;
        }
        return next;
    };

    // 元素内容为空的时候设置一个样式
    Element.SetEmptyFocus = function (obj, cssName) {
        obj = document.id(obj);
        if (obj == null) return;
        if (!cssName) cssName = "empty-focus";

        obj.addEvents({
            "focus": function () {
                this.removeClass(cssName);
            },
            "blur": function (e) {
                if (this.get("value") == "") {
                    this.addClass(cssName);
                }
            }
        });

        (function () {
            obj.fireEvent("blur");
        }).delay(1000);
    };

    // 获取子元素下面所有有名字的字段
    Element.GetData = function (obj) {
        var data = new Object();
        if (!obj) obj = document.id(document.body);
        obj.getElements("[name]").each(function (item) {
            data[item.get("name")] = item.get("value");
        });
        return data;
    };

    // 设置选中之后的样式
    Element.SetCurrent = function (list, cssName, condtion) {
        if (!cssName) cssName = "current";
        if (!condtion) condtion = function (obj) {
            var input = obj.getElement("input[type=radio]");
            if (input == null) return false;
            return input.get("checked");
        };

        list.each(function (item) {
            if (condtion.apply(this, [item])) {
                item.addClass(cssName);
            } else {
                item.removeClass(cssName);
            }
        });
    };

    // 绑定数据到select上
    Element.Select = function (obj, data, options) {
        if (!options) options = new Object();
        if (options.empty) obj.empty();
        if (!options.text) options.text = function (value) { return value; }

        Object.each(data, function (value, key) {
            new Element("option", {
                "text": options.text(value),
                "value": key,
                "selected": options.value && options.value == key ? true : null
            }).inject(obj);
        });
    };
}();

// 自定义添加的事件
!function () {
    // tap事件（移动端专用）
    !function () {
        var disabled;
        console.log("tap");
        Element.Events["tap"] = {
            "onAdd": function (fn) {
                var t = this;
                var target = null;
                t.addEvents({
                    "touchstart": function (e) {
                        target = t;
                    },
                    "touchend": function (e) {
                        if (target) {
                            fn.apply(t, [e]);
                        }
                    },
                    "touchmove": function (e) {
                        target = null;
                    }
                });
            }
        };
    }();
}();

// 基类方法

!function (ns) {
    // 获取网页大小
    ns.getSize = function () {
        /// <summary>
        /// 获取屏幕宽高 返回JSON对象 {x : ,y : , height , top }
        /// x 、 y 为网页可视区域的宽高。 height为网页全部内容的高  top为当前网页被卷去的高度
        /// </summary>

        var height = Math.max(document.documentElement.scrollHeight, document.documentElement.clientHeight);
        if (height > window.screen.availHeight) height = document.documentElement.clientHeight;
        var width = document.documentElement.clientWidth;
        return {
            x: width, y: height,
            width: width,
            height: document.id(document.body).getSize().y,
            top: Math.max(document.body.scrollTop, document.documentElement.scrollTop)
        };
    };

    // 居中一个元素
    ns.center = function (obj, container) {
        /// <summary>
        /// 设置obj居中
        /// </summary> 
        /// <param name="obj">要居中的对象</param>
        /// <param name="container">相对居中的容器。可选项，不填表示body</param>
        obj = document.id(obj);
        if (container == undefined) container = document.body;
        container = document.id(container);
        var body = UI.getSize();
        var position = obj.getCoordinates();
        obj.setStyles({
            "left": (body.x - position.width) / 2,
            "top": (body.height - position.height) / 2 < 0 ? 0 :
                (Browser.ie6 ? (body.height - position.height) / 2 + body.top : (body.height - position.height) / 2)
        });
        return { x: (body.x - position.width) / 2, y: (body.height - position.height) / 2 };
    };

    // 获取当前的声音状态
    ns.SoundState = function () {
        var id = "UI_Sound_Player";
        var value = localStorage.getItem(id);
        return value == null || value == "1";
    };

    // 声音开关 返回当前的状态（true：开启  false：关闭）
    ns.SoundSwitch = function () {
        var id = "UI_Sound_Player";
        var state = !UI.SoundState();
        localStorage.setItem(id, state ? 1 : 0);
        if (!state) {
            var obj = document.id(id);
            if (obj) obj.dispose();
        }
        return state;
    };

    // 播放声音(使用flash播放）
    ns.Sound = function (sound, options) {
        var id = "UI_Sound_Player";
        var obj = document.id(id);
        if (!UI.SoundState()) {
            if (obj) obj.dispose();
            return;
        }
        if (!sound) {
            if (obj) obj.dispose();
            return;
        }
        if (/^\w+$/i.test(sound)) {
            sound = _gPath + "/sound/" + sound + ".mp3";
        }
        var hasVideo = !!(document.createElement('video').canPlayType);
        if (!options) options = {};

        if (obj == null) {
            if (hasVideo) {
                obj = new Element("audio", {
                    "id": id,
                    "autoplay": "autoplay"
                });
                obj.inject(document.body);
            } else {
                if (window["Swiff"]) {
                    obj = new Swiff(_gPath + "/images/dewplayer.swf", {
                        id: id,
                        width: 0,
                        height: 0,
                        param: {
                            "wmode": "transparent"
                        },
                        vars: {
                            mp3: sound,
                            javascript: "on",
                            autostart: "true"
                        },
                        styles: {
                            "position": "absolute",
                            "top": 0,
                            "left": 0,
                            "display": "none"
                        }
                    });
                    obj.inject(document.body);
                }
            }
        }
        if (hasVideo) {
            obj.set("src", sound);
            if (options["loop"]) {
                obj.set("loop", "loop");
            } else {
                obj.removeAttribute("loop");
            }
        } else {
            if (obj && obj.dewset && !reload) {
                try {
                    obj.dewset(sound);
                } catch (ex) {
                    obj.dispose();
                    UI.Sound(sound, ex);
                }
            }
        }
    };

    // 使用百度云的文字语音播放
    ns.SoundText = function (text) {
        //http://fanyi.baidu.com/gettts?lan=zh&text=%E8%AF%B7%E6%B1%82&spd=5&source=web
        var url = "http://fanyi.baidu.com/gettts?lan=zh&spd=5&source=web&text=" + text;
        UI.Sound(url);
    };

    // 产生一个Guid的随机数
    ns.NewGuid = function (format) {
        switch (format) {
            case "N":
            case "n":
                format = "xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx";
                break;
            default:
                format = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx";
                break;
        }
        return format.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };

}(UI);

/*  扩展mootools的方法   */

Object.get = function (obj, key, ignoreCase) {
    /// <summary>
    /// 获取object的指定key对象。
    /// </summary>
    /// <param name="obj">要进行查找的JSON对象</param>
    /// <param name="key">要获取的key。</param>
    /// <param name="ignoreCase">是否区分key的大小写，默认为不区分</param>
    var value = null;
    Object.each(obj, function (v, k) {
        if (key == k || (!ignoreCase && key.toLowerCase() == k.toLowerCase())) value = v;
    });
    return value;
};

// 已经加载的文件列表。
var _importList = new Array();

// 库文件根目录
var _gPath = (function () {
    var str = "/scripts/mootools.js";
    var es = document.getElementsByTagName("script");
    for (var i = 0; i < es.length; i++) {
        var src = es[i].src;
        if (src.toLowerCase().substr(src.length - str.length) == str) {
            return src.substr(0, src.length - str.length);
        }
    }
    return "";
})();

function $import(file, isSync, refresh) {
    /// <summary>
    /// 引用js或者样式库
    /// </summary>
    /// <param name="file">要引用的文件路径 如果无路径符号则引用公共库中的文件</param>
    /// <param name="isSync">Boolean 同步或者异步引用 默认为true</param>
    /// <param name="refresh">Boolean 重新加载时候是否主动刷新 默认为false</param>
    // if (location.href.toLowerCase().contains("/demo/")) { _gPath = "/resources" }
    if (refresh == undefined) refresh = false;
    if (_importList.contains(file)) {
        if (refresh) {
            $$("head script").each(function (item) {
                if (item.get("src") == null) return;
                if (item.get("src").EndWith(file)) {
                    item.dispose();
                }
            });
        } else {
            return;
        }
    }

    _importList.push(file);
    if (isSync == undefined) isSync = true;
    if (!file.contains("/") && file.EndWith(".js")) file = _gPath + "/scripts/" + file;
    if (!file.contains("/") && file.EndWith(".css")) file = _gPath + "/styles/" + file;
    var header = document.getElementsByTagName("head")[0];
    var el = null;
    var fileExt = file.substring(file.lastIndexOf('.'));
    if (fileExt.StartWith(".js")) {
        el = isSync ? "<script language=\"javascript\" type=\"text/javascript\" src=\"" + file + "\"></script>" : new Element("script", {
            "language": "javascript",
            "type": "text/javascript",
            "src": file
        });
    } else if (fileExt.StartWith(".css")) {
        el = isSync ? "<link type=\"text/css\" rel=\"Stylesheet\" href=\"" + file + "\" />" : new Element("link", {
            "type": "text/css",
            "rel": "Stylesheet",
            "href": file
        });
    }
    isSync ? document.write(el) : el.inject($(header));
};

// 设为首页
function SetHome(obj, url) {
    if (vrl == undefined) url = location.host;
    try {
        obj.style.behavior = "url(#default#homepage)";
        obj.setHomePage(url);
    } catch (e) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            } catch (e) {
                alert("\u62B1\u6B49\uFF01\u60A8\u7684\u6D4F\u89C8\u5668\u4E0D\u652F\u6301\u76F4\u63A5\u8BBE\u4E3A\u9996\u9875\u3002\u8BF7\u5728\u6D4F\u89C8\u5668\u5730\u5740\u680F\u8F93\u5165\u201Cabout:config\u201D\u5E76\u56DE\u8F66\u7136\u540E\u5C06[signed.applets.codebase_principal_support]\u8BBE\u7F6E\u4E3A\u201Ctrue\u201D\uFF0C\u70B9\u51FB\u201C\u52A0\u5165\u6536\u85CF\u201D\u540E\u5FFD\u7565\u5B89\u5168\u63D0\u793A\uFF0C\u5373\u53EF\u8BBE\u7F6E\u6210\u529F\u3002");
            }
            var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
            prefs.setCharPref("browser.startup.homepage", url);
        }
    }
}

// 加入收藏夹
function addBookmark(title) {
    if (title == undefined) title = document.title;
    var url = parent.location.href;

    try {
        //IE 
        window.external.addFavorite(url, title);
    } catch (e) {
        try {
            //Firefox 
            window.sidebar.addPanel(title, url, "");
        } catch (e) {
            alert("加入收藏失败，请使用Ctrl+D进行添加,或手动在浏览器里进行设置！", "提示信息");
        }
    }

}

// 设置iframe的高度为自适应自身高度
function setIframeHeight(iframeID) {
    parent.document.all(iframeID).height = parent.document.all(iframeID).style.height = document.id(document.body).getStyle("height").toInt();
}

// 常用的正则表达式验证
var Regex = {
    test: function (type, value) {
        if (!Regex[type]) return null;

        switch (typeof (Regex[type])) {
            case "function":
                return Regex[type].apply(this, [value]);
                break;
            default:
                return Regex[type].test(value);
                break;
        }
        return null;
    },
    "qq": /^[1-9]\d{4,9}$/,     // QQ号
    "email": /^\w[-\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14}$/,     // 电子邮件
    "phone": /^(\d{3,4}-)\d{7,8}(-\d{1,6})?$/,
    "mobile": /^(13\d|14[57]|15[^4,\D]|17[13678]|18\d)\d{8}|170[0589]\d{7}$/,
    "date": /^\d{4}\-[01]?\d\-[0-3]?\d$|^[01]\d\/[0-3]\d\/\d{4}$|^\d{4}年[01]?\d月[0-3]?\d[日号]$/,
    "int": /^\d+$/,
    "integer": /^[1-9][0-9]*$/,
    "number": /^[+-]?[1-9][0-9]*(\.[0-9]+)?([eE][+-][1-9][0-9]*)?$|^[+-]?0?\.[0-9]+([eE][+-][1-9][0-9]*)?$/,
    "numberwithzero": /^[0-9]+$/,
    "money": /^\d+(\.\d{0,2})?$/,
    "alpha": /^[a-zA-Z]+$/,
    "alphanum": /^[a-zA-Z0-9_]+$/,
    "betanum": /^[a-zA-Z0-9-_]+$/,
    "cnid": /^\d{15}$|^\d{17}[0-9a-zA-Z]$/,     // 身份证号码
    "urls": /^(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$/, //URL全称
    "url": /^(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$/, //URL相对路径
    "chinese": /^[\u2E80-\uFE4F]+$/, //中文
    "postal": /^[0-9]{6}$/, //邮编
    "mutiyyyymm": /(20[0-1][0-9]((0[1-9])|(1[0-2]))[\,]?)+$/,  // 多个YYYYMM中间用逗号隔开的方式
    "name": /^([\u4e00-\u9fa5|A-Z|\s]|\u3007)+([\.\uff0e\u00b7\u30fb]?|\u3007?)+([\u4e00-\u9fa5|A-Z|\s]|\u3007)+$/,   //用户名。 允许中文，不允许特殊字符
    "username": /^[a-zA-Z0-9_\u4e00-\u9fa5]{2,16}$/,
    "password": /^.{5,16}$/,   // 密码，5～16位之间
    "realname": /^[\u2E80-\uFE4F]{2,5}$/,  // 中文的真实姓名 2～5位之间
    "passport": /^[a-z0-9]\d{7,8}$/i,
    "company": /^\d{15}$/,  // 企业营业执照号码 15位
    "idcard": function (value) {    // 大陆身份证校验
        if (!/^\d{17}[0-9Xx]|\d{15}$/i.test(value)) return false;
        if (/x/.test(value)) value = value.replace("x", "X");
        var w = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);
        var map = new Array("1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2");
        var sum = 0;
        var index = 0;
        for (var i = 0; i < value.length - 1; i++) {
            sum = sum + value[i] * w[i];
        }
        return map[sum % 11].toLowerCase() == value[value.length - 1].toLowerCase();
    }
};


﻿window["GolbalSetting"] = {
    "Site": null,
    "User": null,
    // ajax请求自定义的头部信息
    "Header": null
};

// betwin 基础框架
if (!window["BW"]) window["BW"] = new Object();

!function (ns) {
    var EVENT_KEY = "BINDEVENT";

    var userAgent = window.navigator.userAgent;

    ns["platform"] = {
        "x5": /x5/.test(userAgent),
        "ios": /iPhone/i.test(userAgent),
        "android": /Android/i.test(userAgent),
        "windows": /Windows/i.test(userAgent),
        "wechat": /MicroMessenger/i.test(userAgent)
    };

    // 执行渲染
    ns["run"] = {
        // 加载一个html页面
        "control": function () {
            var t = this;
            var success = function (html) {
                t.dom.container.set("html", html);
                t.callback(html);
                ns.Utils.bindAction.apply(t);
            };
            var html = null;
            var cacheKey = "control:" + t.options.action;
            if (t.options.cache) {
                html = ns.Utils.getStore(cacheKey);
            }
            if (html) {
                success.apply(t, [html]);
            } else {
                new Request.HTML({
                    "url": t.options.action,
                    "noCache": !t.options.cache,
                    "onRequest": function () {
                        if (ns.loading["control"]) ns.loading["control"].apply(t, [true]);
                    },
                    "onComplete": function () {
                        if (ns.loading["control"]) ns.loading["control"].apply(t, [false]);
                    },
                    "onSuccess": function (responseTree, responseElements, responseHTML, responseJavaScript) {
                        if (t.options.cache) ns.Utils.setStore(cacheKey, responseHTML);
                        success.apply(t, [responseHTML])
                    }
                }).get();
            }
        },
        // AJAX请求
        "ajax": function (callback) {
            var t = this;
            var data = t.getPostData();

            var success = function (result) {
                t.callback(result, callback);
                // ns.Utils.bindAction.apply(t);
            };

            var cacheKey = "ajax:" + t.options.action + "?" + Object.toQueryString(data);
            var result = null;
            if (t.options.cache) {
                result = ns.Utils.getStore(cacheKey);
            }
            if (result) {
                success.apply(t, [result]);
            } else {
                new Request.JSON({
                    "url": t.options.action,
                    "headers": GolbalSetting.Header || {},
                    "onRequest": function () {
                        if (ns.loading["ajax"]) ns.loading["ajax"].apply(t, [true]);
                    },
                    "onComplete": function () {
                        if (ns.loading["ajax"]) ns.loading["ajax"].apply(t, [false]);
                    },
                    "onFailure": function (xhr) {
                        var msg = null;
                        switch (xhr.status) {
                            case 503:
                                msg = null;
                                break;
                            default:
                                msg = xhr.response;
                                break;
                        }
                        if (msg) {
                            msg = msg.replace(/\<a .+?\<\/a\>/g, "BetWin 2.0.1");
                            if (ns.Tip) new BW.Tip(msg);
                        }
                    },
                    "onSuccess": function (result) {
                        success.apply(t, [result]);
                        if (t.options.cache) ns.Utils.setStore(cacheKey, result);
                    }
                }).post(data);
            }
        },
        // 表单提交
        "form": function () {
            var t = this;
            if (t.dom.element.get("tag") == "form") {
                t.dom.element.set({
                    "events": {
                        "submit": function (e) {
                            e.stop();
                            var request = this.get("send");
                            Object.forEach(GolbalSetting.Header || {}, function (value, name) {
                                request.setHeader(name, value);
                            });
                            request.send({ "url": t.options.action });
                        }
                    },
                    "send": {
                        "headers": GolbalSetting.Header || {},
                        "onRequest": function () {
                            if (ns.loading["form"]) ns.loading["form"].apply(t, [true]);
                        },
                        "onComplete": function () {
                            if (ns.loading["form"]) ns.loading["form"].apply(t, [false]);
                        },
                        "onSuccess": function (result) {
                            var result = JSON.decode(result);
                            if (result && result.success) {
                                t.dom.element.reset();
                            }
                            t.callback(result);
                        }
                    }
                });
            }
        },
        // 列表
        "list": function () {

        }
    };

    // BetWin 的工具类
    ns["Utils"] = {
        // 获取本地存储值
        "getStore": function (key) {
            if (!localStorage) return;
            var result = localStorage.getItem(key);
            if (!result) return null;
            if (key.indexOf("ajax:") == 0 || key.indexOf("json:") == 0) return JSON.decode(result);
            return result;
        },
        // 设定本地存储值
        "setStore": function (key, value) {
            if (!localStorage) return;
            if (typeof (value) == "object") value = JSON.encode(value);
            localStorage.setItem(key, value);
        },
        // 清理缓存（自动判断移动端）
        "clearcache": function (callback) {
            // 清除本地存储
            !function () {
                if (!window["localStorage"]) return;
                for (var i = localStorage.length - 1; i >= 0; i--) {
                    var key = localStorage.key(i);
                    ["control", "ajax"].each(function (item) {
                        if (key.indexOf(item + ":") == 0) localStorage.removeItem(key);
                    });
                }
            }();

            // 执行WeX5的清除事件
            if (ns.platform.x5) {

            }
            if (callback) callback.apply(this);
        },
        // 自动绑定下级的bind事件
        "bindAction": function () {
            var t = this;
            var el = document.body;
            if (t && t.dom && t.dom.container) el = t.dom.container;
            el.getElements("[data-bind-action]").each(function (item) {
                new ns.Bind(item);
            });
        }
    };

    // 绑定元素上的数据
    ns["Bind"] = new Class({
        Implements: [Events, Options],
        "options": {
            // 提交地址
            "action": null,
            // 渲染类型 control | ajax | list | form
            "type": null,
            // 触发数据加载的动作    load | click | submit | confirm
            "event": null,
            // 页面加载完成之后执行
            "load": null,
            // 自定义的回调函数
            "callback": null,
            // 目标对象，默认为自己
            "target": null,
            // 自定义的等待方法
            "loading": null,
            // dom元素上设定的要传输的值
            "data": null,
            // 是否启用本地缓存（仅限于control类型）
            "cache": null,
            // 不自动执行
            "stop": false,
            "confirm": "您确认要进行该操作吗？"
        },
        "dom": {
            // 原始对象
            "element": null,
            // 指定的页面对象
            "container": null,
            // 指定的搜索框对象
            "searchbox": null,
            // 分页控件
            "pagesplit": null,
            // 页面子元素
            "elements": new Object()
        },
        "data": {
            // 要上传的数据
            "post": null
        },
        // 从元素上加载参数值
        "loadOptions": function (el) {
            var t = this;
            if (!el) el = t.dom.element;
            if (!el) return;
            var prefix = "data-bind-";
            var regex = new RegExp("^" + prefix);
            for (var i = 0; i < el.attributes.length; i++) {
                var att = el.attributes[i];
                if (regex.test(att.name)) {
                    var name = att.name.substr(prefix.length);
                    if (!t.options[name]) t.options[name] = att.value;
                }
            }
        },
        // 设定数据
        "setData": function (data) {
            var t = this;
            t.data.post = data;
        },
        // 获取要提交的数据
        "getPostData": function () {
            var t = this;
            var data = new Object();
            if (t.options.data) {
                t.options.data.split(',').each(function (item) {
                    switch (item) {
                        case "parent":
                            var parent = t.dom.element.getParent("[data-bind-post]");
                            if (parent != null) {
                                data = Object.merge(data, parent.get("data-bind-post").parseQueryString());
                            }
                            break;
                        case "url":
                            var url = location.search;
                            if (url && url.indexOf("?") == 0) {
                                url = url.substr(1);
                                data = Object.merge(data, url.parseQueryString());
                            }
                            break;
                        case "form":
                            var form = t.dom.element.getParent("form,[data-form]");
                            if (!form) {
                                var parent = t.dom.element;
                                while (parent != null) {
                                    form = parent.getElement("form,[data-form]");
                                    if (!form) break;
                                    parent = parent.getParent();
                                }
                            }
                            if (form) {
                                data = Object.merge(data, Element.GetData(form));
                            } else {

                            }
                            break;
                        default:
                            if (item.contains("=")) {
                                data = Object.merge(data, item.parseQueryString());
                            }
                            break;
                    }
                });
            }
            if (t.dom.searchbox) data = Object.merge(data, Element.GetData(t.dom.searchbox));
            if (t.data.post) data = Object.merge(data, t.data.post);
            return data;
        },
        // 构造函数
        "initialize": function (el, options) {
            var t = this;
            t.dom.element = $(el);
            t.loadOptions();
            t.setOptions(options);

            if (t.options.target) {
                t.dom.container = $(t.options.target);
            } else {
                t.dom.container = t.dom.element;
            }

            if (t.options.load) {
                t.options.load.split(',').each(function (name) {
                    if (ns.load[name]) ns.load[name].apply(t);
                });
            }
            if (!t.options.stop) t.fire();

            switch (t.options.event) {
                case "click":
                case "tap":
                    t.dom.element.addEvent(t.options.event, function () {
                        t.fire();
                    });
                    break;
                case "confirm":
                    t.dom.element.addEvent("click", function () {
                        new BW.Tip(t.options.confirm, {
                            "callback": function (e) {
                                if (e.type == "confirm") t.fire();
                            }
                        });
                    });
                    break;
            }

            t.dom.element.store(EVENT_KEY, t);
        },
        "fire": function () {
            var t = this;
            if (ns.run[t.options.type]) {
                ns.run[t.options.type].apply(t);
            }
        },
        // 执行回调事件
        "callback": function (result, callback) {
            var t = this;
            if (!callback) callback = t.options.callback;
            if (!callback) return;
            // 全局的错误处理
            if (typeof (result) == "object" && !result.success && BW.callback["golbal-error"]) {
                BW.callback["golbal-error"].apply(t, [result]);
            }
            callback.split(",").each(function (name) {
                if (ns.callback[name]) ns.callback[name].apply(t, [result]);
            });
        }
    });

    // 获取绑定的事件
    Element.prototype.getBindEvent = function () {
        return this.retrieve(EVENT_KEY);
    };

    // 执行绑定的事件
    Element.prototype.fire = function () {
        var t = this.getBindEvent();
        if (t) t.fire();
    };

}(BW);


// 公共的等待效果
if (!BW.loading) BW.loading = new Object();
!function (ns) {

    var loading = function (type, show) {
        var t = this;
        var cssName = "loading-" + type;
        if (show) {
            t.dom.container.addClass(cssName);
        } else {
            t.dom.container.removeClass(cssName);
        }
    };

    ns["ajax"] = function (show) {
        var t = this;
        loading.apply(t, ["ajax", show]);
    };

    ns["form"] = function (show) {
        var t = this;
        loading.apply(t, ["form", show]);
    };

    ns["control"] = function (show) {
        var t = this;
        loading.apply(t, ["control", show]);
    };

}(BW.loading)


window.addEvent("domready", function () {
    $$("[data-bind-action]").each(function (item) {
        new BW.Bind(item);
    });

    // 给body添加当前平台的标识
    !function () {
        Object.forEach(BW.platform, function (value, key) {
            if (value) document.body.addClass("bw-platform-" + key);
        });
    }();

});﻿if (!BW.callback) BW.callback = new Object();
if (!BW.load) BW.load = new Object();

// 系统公用的初始化系统
!function (ns) {
    // 绑定标记了 data-dom 的元素（存在重复元素返回Array）
    ns["dom"] = function () {
        var t = this;
        if (!t.dom.elements) t.dom.elements = new Object();
        t.dom.element.getElements("[data-dom]").each(function (item) {
            var name = item.get("data-dom");
            if (!t.dom.elements[name]) {
                t.dom.elements[name] = item;
            } else if (t.dom.elements[name].length) {
                t.dom.elements[name].push(item);
            } else {
                t.dom.elements[name] = [t.dom.elements[name]];
                t.dom.elements[name].push(item);
            }
        });
    };

    // 绑定搜索控件
    ns["search"] = function () {
        var t = this;
        var searchbox = null;
        var parent = t.dom.element;
        while (parent) {
            if (parent.get("tag") == "form") {
                searchbox = parent;
                break;
            }
            var form = parent.getElement("form");
            if (form) {
                searchbox = form;
                break;
            }
            parent = parent.getParent();
        }
        if (!searchbox) return;
        t.dom.searchbox = searchbox;
        searchbox.addEvent("submit", function (e) {
            t.fire();
            e.stop();
        });
    };

}(BW.load);

// 全局设定
!function (ns) {
    // 设定站点信息
    ns["setting-site"] = function (result) {
        var t = this;
        if (!result.success) return;
        GolbalSetting.Site = result.info;
        console.log(BW.Utils.setStore);
        BW.Utils.setStore("GolbalSetting.Site", GolbalSetting.Site);
    };

}(BW.callback);
// 控件
!function (ns) {
    ns["control-toggle"] = function () {
        var t = this;
        var dom = new Object();
        t.dom.container.getElements("[data-toggle-id]").each(function (item) {
            dom[item.get("data-toggle-id")] = item;
        });
        t.dom.container.getElements("[data-toggle-target]").addEvent("click", function () {
            var target = this.get("data-toggle-target");
            Object.forEach(dom, function (item, key) {
                if (key == target) return;
                var className = item.get("data-toggle-name") || "show";
                if (item.hasClass(className)) item.removeClass(className);
            });
            if (!dom[target]) return;
            var className = dom[target].get("data-toggle-name") || "show";
            dom[target].toggleClass(className);
        });
    };

    // 绑定select控件
    ns["control-select"] = function (result) {
        var t = this;
        if (!result.success) return;
        switch (t.dom.element.get("tag")) {
            case "select":
                switch (typeof (result.info)) {
                    case "object":
                        Object.forEach(result.info, function (value, key) {
                            var element = null;
                            switch (typeof (value)) {
                                case "string":
                                    element = new Element("option", {
                                        "value": value,
                                        "text": key
                                    });
                                    break;
                                case "object":
                                    element = new Element("optgroup", {
                                        "label": key
                                    });
                                    Object.forEach(value, function (v, k) {
                                        new Element("option", {
                                            "value": k,
                                            "text": v
                                        }).inject(element);
                                    });
                                    break;
                            }
                            if (!element) return;

                            element.inject(t.dom.element);
                        });
                        break;
                }

                break;
        }
    };

    // UI控件
    ns["control-ui"] = function () {
        var t = this;

        t.dom.element.getElements("input[data-type]").each(function (item) {
            switch (item.get("data-type")) {
                case "date":
                    item.set("title", "日历控件");
                    break;
            }
        });
    };

    // 向上滚动控件（配合list回调使用）
    ns["control-list-up"] = function () {
        var t = this;
        var height = t.dom.element.getStyle("height").toInt();
        var obj = t.dom.element.getElement("[data-list-element]");

        if (!obj) return;
        var list = t.dom.element.getElements("[data-list-element] > *");
        if (list.length <= 1) return;
        var index = 0;
        var over = false;
        obj.addEvents({
            "mouseover": function () {
                over = true;
            },
            "mouseout": function () {
                over = false;
            }
        })
        var fx = function () {
            var top = (index % list.length) * height * -1;
            if (top == 0) {
                obj.addClass("bw-animate-stop");
            } else {
                obj.removeClass("bw-animate-stop");
            }
            (function () {
                if (!over) {
                    obj.setStyle("margin-top", top);
                    index++;
                }
                fx.delay(1000);
            }).delay(50);
        };
        console.log(obj);
        fx.apply();
    };

}(BW.callback);

// 表单回调
!function (ns) {
    // 提示框（自动关闭）
    ns["form-tip"] = function (result) {
        var t = this;
        new BW.Tip(result.msg, {
            "type": "tip",
            "callback": function (e) {
                var tip = this;
                var callback = t.dom.element.get("data-bind-form-tip");
                if (callback) t.callback(result, callback);
            },
            "target": t.dom.element.get("data-bind-form-tip-target") == "body" ? null : t.dom.element,
            "closetime": t.dom.element.get("data-bind-form-tip-closetime")
        });
    };

    // 弹出确认框
    ns["form-alert"] = function (result) {
        var t = this;
        new BW.Tip(result.msg, {
            "type": "alert",
            "callback": function (e) {
                if (result.success) {
                    var callback = t.dom.element.get("data-bind-form-tip");
                    if (callback) t.callback(result, callback);
                }
            }
        });
    };

    // 填充内容
    ns["form-fill"] = function (result) {
        var t = this;
        if (!result.success) return;
        var data = result.info;
        t.dom.container.getElements("[data-name],[name]").each(function (item) {
            var name = item.get("data-name") || item.get("name");
            var format = item.get("data-format");
            var value = data[name];
            if (!value) return;
            if (format && htmlFunction[format]) value = htmlFunction[format](value);
            switch (item.get("tag")) {
                case "img":
                    item.set("src", value);
                    break;
                case "input":
                case "textarea":
                    item.set("value", value);
                    break;
                case "section":
                    item.set("html", value);
                    break;
                default:
                    item.set("text", value);
                    break;
            }
        });
    };

}(BW.callback);

// Diag回调
!function (ns) {
    ns["diag-close"] = function (result) {
        var t = this;
        var diag = t.dom.element.getParent(".bw-diag");
        if (!diag) return;

        var diagObj = diag.retrieve("diag");
        if (!diagObj) return;

        diagObj.close();
    };
}(BW.callback);

!function (ns) {
    var LIST_TEMPLATE = "LIST_TEMPLATE";
    var HTML_TEMPLATE = "HTML_TEMPLATE";

    ns["list"] = function (result) {
        var t = this;
        var element = t.dom.container.getElement("[data-list-element]");
        if (!element) {
            new BW.Tip("没有指定data-list-element元素");
            return;
        }
        var template = element.retrieve(LIST_TEMPLATE);
        if (!template) {
            template = element.get("html");
            element.store(LIST_TEMPLATE, template);
        }
        if (!result.success) return;
        var list = result.info.list;
        if (!list && result.info.length) {
            list = result.info;
        }
        var html = new Array();
        if (result.info.RecordCount == 0) {
            switch (element.get("tag")) {
                case "tbody":
                    var thead = element.getPrevious("thead");
                    if (thead) {
                        var thead_length = thead.getElements("th").length;
                        html.push("<tr><td colspan=\"" + thead_length + "\"><p class=\"empty\"></p></td></tr>");
                    }
                    break;
            }
        } else {
            list.each(function (item) {
                var content = template.toHtml(item);
                html.push(content);
            });
        }
        element.set("html", html.join(""));
    };

    // 分页控件
    ns["pagesplit"] = function (result) {
        var t = this;
        var pagesplit = t.dom.container.getNext(".bw-pagesplit");
        if (!pagesplit) {
            pagesplit = new Element("div", {
                "class": "bw-pagesplit",
                "events": {
                    "click": function (e) {
                        var target = $(e.target);
                        var page = target.get("data-page");
                        if (!page) return;
                        t.setData({
                            "PageIndex": page
                        });
                        t.fire();
                        t.setData();
                        var top = t.dom.container.getPosition().y;
                        window.scrollTo(0, top);
                    }
                }
            });
            pagesplit.inject(t.dom.container, "after");
        };
        pagesplit.empty();
        if (!result.info.RecordCount) return;

        var maxpage = result.info.RecordCount % result.info.PageSize == 0 ? result.info.RecordCount / result.info.PageSize : Math.floor(result.info.RecordCount / result.info.PageSize) + 1;

        var pageindex = parseInt(result.info.PageIndex);
        console.log(typeof pageindex);
        var list = new Array();
        list.push({ "name": "第一页", "page": 1 });
        if (pageindex != 1) list.push({ "name": "上一页", "page": pageindex - 1 });
        for (var i = Math.max(1, pageindex - 3); i <= Math.min(maxpage, Math.max(7, pageindex + 3)); i++) {
            list.push({ "name": i, "page": i, "active": i == pageindex })
        };
        if (pageindex != maxpage) list.push({ "name": "下一页", "page": pageindex + 1 });
        list.push({ "name": "最后页", "page": maxpage });

        list.each(function (item) {
            new Element("a", {
                "href": "javascript:",
                "class": item.active ? "active" : "",
                "text": item.name,
                "data-page": item.page
            }).inject(pagesplit);
        });
    };

    // html内容渲染
    ns["html"] = function (result) {
        var t = this;
        var html = t.dom.container.retrieve(HTML_TEMPLATE, t.dom.container.get("html"));
        t.dom.container.store(HTML_TEMPLATE, html);
        if (result.info) {
            t.dom.container = t.dom.container.set("html", html.toHtml(result.info));
        }
    }

}(BW.callback);
﻿// 弹出框  配合 css/betwin.tip.css 使用

!function (ns) {
    ns["Tip"] = new Class({
        "Implements": [Events, Options],
        "options": {
            // 弹窗类型 alert | confirm | tip
            "type": "alert",
            // 是否启用遮罩
            "mask": true,
            "target": $(document.body),
            "callback": function (e) {

            },
            // 自动关闭的时间
            "closetime": null
        },
        "dom": {
            "mask": null,
            "alert": null
        },
        "initialize": function (msg, options) {
            var t = this;
            t.setOptions(options);
            if (!t.options.target) t.options.target = $(document.body);
            t.dom.alert = new Element("div", {
                "class": "bw-tip hide bw-tip-" + t.options.type,
                "html": "<content>" + msg + "</content>"
            });
            t.dom.alert.inject(t.options.target);
            t.dom.alert.removeClass.delay(10, t.dom.alert, ["hide"]);

            var nav = new Element("nav", {
                "events": {
                    "click": function (e) {
                        var obj = $(e.target);
                        if (obj.get("tag") == "a") {
                            t.options.callback.delay(100, t, [{
                                "type": obj.get("class")
                            }]);
                            t.dispose();
                        }
                    }
                }
            });
            switch (t.options.type) {
                case "alert":
                    new Element("a", {
                        "class": "confirm",
                        "text": "确定"
                    }).inject(nav);
                    nav.inject(t.dom.alert);
                    break;
                case "tip":
                    if (!t.options.closetime) t.options.closetime = 3000;
                    t.dom.alert.addEvent("click", function () {
                        t.options.closetime = 0;
                        t.options.callback.apply(t);
                        t.dispose();
                    });
                    break;
                case "confirm":
                    new Element("a", {
                        "class": "confirm",
                        "text": "确定"
                    }).inject(nav);
                    new Element("a", {
                        "class": "cancel",
                        "text": "取消"
                    }).inject(nav);
                    nav.inject(t.dom.alert);
                    break;
            }

            if (t.options.mask) {
                t.dom.mask = new Element("div", {
                    "class": "tip-mask"
                });
                t.dom.mask.inject(t.dom.alert, "after");
            }

            if (t.options.closetime) {
                (function () {
                    if (!t.options.closetime) return;
                    t.options.callback.apply(t);
                    t.dispose.apply(t);
                }).delay(t.options.closetime);

            }
        },
        // 关闭弹窗
        "dispose": function () {
            var t = this;
            if (t.dom.mask) {
                t.dom.mask.addClass("hide");
                t.dom.mask.dispose.delay(250, t.dom.mask);
            }
            if (t.dom.alert) {
                t.dom.alert.addClass("hide");
                t.dom.alert.dispose.delay(250, t.dom.alert);
            }
        }
    });
}(BW);﻿// 弹出框

!function (ns) {
    var DiagObj = new Object();

    ns.Diag = new Class({
        "Implements": [Events, Options],
        "options": {
            // 窗体的名字（防止重复打开）
            "name": null,
            // 是否有遮罩
            "mask": true,
            // 点击遮罩关闭弹出层
            "maskclose": false,
            // 宽度
            "width": 640,
            // 高度
            "height": 480,
            // 是否允许改变大小
            "resize": false,
            // 要加载的目标路径
            "src": null,
            // 要加载的目标类型 control | frame
            "type": "control",
            // 是否使用标题栏
            "title": null,
            "data": null,
            // 是否有关闭按钮
            "close": true,
            // 自定义的样式
            "cssname": null
        },
        "initialize": function (options) {
            var t = this;
            t.setOptions(options);
            if (!t.options.name) return;


            if (DiagObj[t.options.name]) {
                DiagObj[t.options.name].close();
                //DiagObj[t.options.name].element.diag.addClass("shake");
                //(function () {
                //    DiagObj[t.options.name].element.diag.removeClass("shake");
                //}).delay(200);
                //return;
            }

            if (t.options.mask) {
                t.element.mask = new Element("div", {
                    "class": "bw-mask hide " + (t.options.mask ? "" : "hide"),
                    "events": {
                        "click": function () {
                            if (t.options.maskclose) t.close();
                        }
                    }
                });
                t.element.mask.inject(document.body);
                t.element.mask.removeClass.delay(10, t.element.mask, ["hide"]);
            }

            t.element.diag = new Element("div", {
                "class": "bw-diag " + (t.options.cssname || ""),
                "events": {
                    "mousewheel": function (e) {
                        e.stopPropagation();
                    }
                },
                "styles": {
                    "width": t.options.width,
                    "height": t.options.height
                }
            });
            t.element.diag.inject(document.body);

            if (t.options.title) {
                t.element["diag-title"] = new Element("div", {
                    "class": "bw-diag-title",
                    "html": "<h3>" + t.options.title + "</h3>"
                });
                t.element["diag-title"].inject(t.element.diag);
            }

            if (t.options.close) {
                new Element("a", {
                    "href": "javascript:",
                    "class": "bw-diag-close",
                    "events": {
                        "click": function () {
                            t.close();
                        }
                    }
                }).inject(t.element.diag);
            }

            t.element["diag-content"] = new Element("div", {
                "class": "bw-diag-content"
            });
            t.element["diag-content"].inject(t.element.diag);


            switch (t.options.type) {
                case "control":
                    if (t.options.data) {
                        switch (typeof (t.options.data)) {
                            case "string":
                                t.element["diag-content"].set("data-bind-post", t.options.data);
                                break;
                            case "object":
                                t.element["diag-content"].set("data-bind-post", Object.toQueryString(t.options.data));
                                break;
                        }
                    }
                    new ns.Bind(t.element["diag-content"], {
                        "action": t.options.src,
                        "type": "control"
                    });
                    break;
            }

            DiagObj[t.options.name] = t;

            t.element.diag.store("diag", t);

        },
        // 弹出框的dom元素
        "element": {
            // 遮罩元素
            "mask": null,
            // 弹出框元素
            "diag": null,
            // 标题栏
            "diag-title": null,
            // 内容
            "diag-content": null
        },
        // 打开窗体
        "open": function () {
            var t = this;
        },
        // 关闭窗体
        "close": function () {
            var t = this;
            if (t.element.diag) {
                t.element.diag.addClass("hide");
                t.element.diag.dispose.delay(250, t.element.diag);
            }
            if (t.element.mask) {
                t.element.mask.addClass("hide");
                t.element.mask.dispose.delay(250, t.element.mask);
            }
            if (DiagObj[t.options.name]) delete DiagObj[t.options.name];
        }
    });

    // 对外公开的参数
    ns.DiagOpen = function (name, option) {
        option.name = name;
        new BW.Diag(option);
    }
}(BW);﻿if (!window["BW"]) window["BW"] = new Object();

// 框架
!function(ns) {

    var STORE_KEY = "FRAMES";

    ns.Frame = new Class({
        Implements: [Events, Options],
        "options": {
            // 外层元素
            "container": null,
            // 要显示的数量
            "count": 0,
            // 关闭页面的回调事件
            "unload": function() { }
        },
        "dom": {
            "container": null,
            "frames": function() {
                var t = this;
                var container = typeof (t.dom.container) == "function" ? t.dom.container() : t.dom.container;
                var frame = container.getElement(".bw-frames");
                if (!frame) {
                    frame = new Element("div", {
                        "class": "bw-frames"
                    });
                    frame.inject(container);
                }
                return frame;
            }
        },
        "data": {
            // 已经打开的窗口
            "frame": [],//[{name:obj}],
            // 根据名字查找窗体
            "getFrame": function(name) {
                var t = this;
                var obj = null;
                t.data.frame.each(function(item) {
                    if (obj) return;
                    if (item["name"] == name) obj = item.obj;
                });
                return obj;
            },
            // 打开一个窗口
            "add": function(name, obj) {
                var t = this;
                if (t.options.count == 1 && t.data.frame.length != 0) {
                    t.data.frame[0].obj.dispose();
                    t.data.frame = [{ "name": name, "obj": obj }];
                    return;
                }
                t.data.frame.push({ "name": name, "obj": obj });
                if (t.data.frame.length == 1) {
                    obj.addClass("active");
                } else {
                    (function() {
                        var pre = t.data.frame[t.data.frame.length - 2].obj;
                        if (pre && pre.hasClass("active")) pre.removeClass("active");
                        obj.addClass("active");
                    }).delay(10);
                }
            },
            // 移除数据（name以及name以后的）
            "remove": function(name) {
                var t = this;
                var frameIndex = name ? -1 : t.data.frame.length - 1;
                t.data.frame.each(function(item, index) {
                    if (frameIndex != -1) return;
                    if (item.name == name) frameIndex = index + 1;
                });
                if (frameIndex < 0) return;
                t.data.frame.each(function(item, index) {
                    if (index < frameIndex) return;
                    var obj = item.obj;
                    if (obj.hasClass("active")) {
                        obj.removeClass("active");
                        (function() {
                            obj.dispose();
                        }).delay(500);
                    } else {
                        obj.dispose();
                    }
                    t.data.frame[index] = null;
                });
                t.data.frame = t.data.frame.clean();
                t.data.frame.getLast().obj.addClass("active");
            }
        },
        "initialize": function(options) {
            var t = this;
            t.setOptions(options);
            t.dom.container = t.options.container;
        },
        // 新开一个窗口
        "open": function(name, action, callback, data) {
            var t = this;
            if (!name) name = action;
            callback = callback ? callback.split(',') : [];
            callback.push("control-frame");
            var frame = t.data.getFrame.apply(t, [name]);
            if (data && typeof (data) == "object") data = Object.toQueryString(data);
            if (!frame) {
                frame = new Element("div", {
                    "data-frame-name": name,
                    "class": "frame-item",
                    "data-bind-action": action,
                    "data-bind-type": "control",
                    "data-bind-stop": true,
                    "data-bind-callback": callback.length == 0 ? null : callback.join(","),
                    "data-bind-post": data || null
                });
                frame.store(STORE_KEY, t);
                new BW.Bind(frame);
                t.data.add.apply(t, [name, frame]);
            } else {
                t.close(name);
                return;
            }
            frame.inject(t.dom.frames.apply(t));
            frame.fire();
            //if (t.options.count == 1) {
            //    Object.forEach(t.data.frame, function (item, itemName) {
            //        if (itemName != name) item.dispose();
            //    });
            //}
        },
        // 未指定name的时候关闭最后一个，指定了name则关闭从包含name以及之后的所有
        "close": function(name) {
            var t = this;
            t.data.remove.apply(t, [name]);
        }
    });

    // 框架的回调代码
    ns.callback["control-frame"] = function() {
        var t = this;
        t.dom.element.getElements("[data-frame]").addEvent("tap", function() {
            var item = this;
            var name = item.get("data-frame");
            if (!name) return;
            var frame = t.dom.element.retrieve(STORE_KEY);
            switch (name) {
                case "back":
                    if (frame) frame.close.apply(frame);
                    break;
                case "link":
                    if (frame) frame.open.apply(frame, [item.get("data-frame-link-name"), item.get("data-frame-link")]);
                    break;
            }
        });
        if (ns.OffCanvasObj) ns.OffCanvasObj.close();
    };

}(BW);﻿// 移动端使用 侧滑菜单

!function (ns) {
    // 当前打开的侧滑窗体
    ns.OffCanvasObj = null;

    ns.OffCanvas = new Class({
        "Implements": [Events, Options],
        "options": {
            // 可自定义的遮罩样式
            "mask": null,
            // 菜单大小
            "size": "auto",
            // 附带的数据
            "data": null,
            // 自定义样式
            "cssname": null,
            // 动作类型(bottom|top|left|right)
            "action": "bottom",
            // 要加载的路径地址
            "src": null
        },
        "dom": {
            "mask": null,
            "canvas": null
        },
        "initialize": function (options) {
            if (ns.OffCanvasObj) ns.OffCanvasObj.close();
            var t = ns.OffCanvasObj = this;
            t.setOptions(options);

            t.dom.mask = new Element("div", {
                "class": "bw-mask " + (t.options.mask || ""),
                "events": {
                    "click": function () {
                        console.log("关闭");
                        t.close();
                    }
                }
            });
            var data = t.options.data;
            if (data && typeof (data) == "object") {
                data = Object.toQueryString(t.options.data);
            }
            t.dom.canvas = new Element("div", {
                "class": "bw-offcanvas " + t.options.action + " " + (t.options.cssname || ""),
                "data-bind-action": t.options.src,
                "data-bind-type": "control",
                "data-bind-post": data || null,
                "events": {
                    "click": function (e) {
                        e.stopPropagation();
                    }
                }
            });
            if (t.options.size) {
                switch (t.options.action) {
                    case "top":
                    case "bottom":
                        t.dom.canvas.setStyle("height", t.options.size);
                        break;
                    case "left":
                    case "right":
                        t.dom.canvas.setStyle("width", t.options.size);
                        break;
                }
            }
            t.dom.canvas.inject(t.dom.mask);
            t.dom.mask.inject(document.body);
            new BW.Bind(t.dom.canvas);
            t.dom.mask.addClass.delay(50, t.dom.mask, ["active"]);
        },
        "close": function () {
            var t = this;
            t.dom.mask.removeClass("active");
            t.dom.mask.dispose.delay(250, t.dom.mask);
            ns.OffCanvasObj = null;
        }
    });

}(BW);﻿// 切换效果
!function (ns) {
    ns["Tab"] = new Class({
        "Implements": [Events, Options],
        "options": {
            "nav": new Array(),
            "frame": new Array(),
            "activeName": "active",
            "event": "click",
            "display": 0
        },
        "initialize": function (options) {
            var t = this;
            t.setOptions(options);

            var nav_active = null;
            var frame_active = null;
            t.options.nav.each(function (nav, index) {
                nav.addEvent(t.options.event, function () {
                    if (nav_active == nav) return;
                    if (nav_active) nav_active.removeClass(t.options.activeName);
                    if (frame_active) frame_active.removeClass(t.options.activeName);
                    nav_active = nav;
                    frame_active = t.options.frame[index];
                    nav_active.addClass("active");
                    frame_active.addClass("active");
                    frame_active.fire.delay(100, frame_active);
                });
                if (index == t.options.display) {
                    nav.fireEvent(t.options.event);
                }
            });
        }
    });
}(BW);