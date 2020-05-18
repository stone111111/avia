DEMO:http://www.jq22.com/yanshi10850

// Simple list
var list = document.getElementById("my-ui-list");
Sortable.create(list); // That's all.


// Grouping
var foo = document.getElementById("foo");
Sortable.create(foo, { group: "omega" });

var bar = document.getElementById("bar");
Sortable.create(bar, { group: "omega" });


// Or
var container = document.getElementById("multi");
var sort = Sortable.create(container, {
  animation: 150, // ms, animation speed moving items when sorting, `0` — without animation
  handle: ".tile__title", // Restricts sort start click/touch to the specified element
  draggable: ".tile", // Specifies which items inside the element should be sortable
  onUpdate: function (evt/**Event*/){
     var item = evt.item; // the current dragged HTMLElement
  }
});

// ..
sort.destroy();


// Editable list
var editableList = Sortable.create(editable, {
  filter: '.js-remove',
  onFilter: function (evt) {
    var el = editableList.closest(evt.item); // get dragged item
    el && el.parentNode.removeChild(el);
  }
});