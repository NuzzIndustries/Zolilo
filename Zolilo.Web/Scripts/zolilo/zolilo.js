/*  zolilo.js
*   
*
*
*
*
*/
/* Simple JavaScript Inheritance
* By John Resig http://ejohn.org/
* MIT Licensed.
*/
// Inspired by base2 and Prototype
//http://ejohn.org/blog/simple-javascript-inheritance/
(function ()
{
    var initializing = false, fnTest = /xyz/.test(function () { xyz; }) ? /\b_super\b/ : /.*/;
    // The base Class implementation (does nothing)
    this.Class = function () { };

    // Create a new Class that inherits from this class
    Class.extend = function (prop)
    {
        var _super = this.prototype;

        // Instantiate a base class (but only create the instance,
        // don't run the init constructor)
        initializing = true;
        var prototype = new this();
        initializing = false;

        // Copy the properties over onto the new prototype
        for (var name in prop)
        {
            // Check if we're overwriting an existing function
            prototype[name] = typeof prop[name] == "function" &&
        typeof _super[name] == "function" && fnTest.test(prop[name]) ?
        (function (name, fn)
        {
            return function ()
            {
                var tmp = this._super;

                // Add a new ._super() method that is the same method
                // but on the super-class
                this._super = _super[name];

                // The method only need to be bound temporarily, so we
                // remove it when we're done executing
                var ret = fn.apply(this, arguments);
                this._super = tmp;

                return ret;
            };
        })(name, prop[name]) :
        prop[name];
        }

        // The dummy class constructor
        function Class()
        {
            // All construction is actually done in the init method
            if (!initializing && this.init)
                this.init.apply(this, arguments);
        }

        // Populate our constructed prototype object
        Class.prototype = prototype;

        // Enforce the constructor to be what we expect
        Class.prototype.constructor = Class;

        // And make this class extendable
        Class.extend = arguments.callee;

        return Class;
    };
})();
//End inheritance snippet


function Loading(toggle)
{
    var elem = document.getElementById("loadingdiv");
    if (toggle)
    {
        elem.style.display = "inline";
    }
    else
    {
        elem.style.display = "none";
    }
}

//This function must be on the page or else the control will get loaded multiple times
function ZoliloInnerControlExists()
{
    return true;
}

function ZoliloPageMaster()
{
}

function ShowClickEvents(clickEvents)
{
    jQuery.each(clickEvents, function (key, handlerObj)
    {
        console.log(handlerObj.handler) // prints "function() { console.log('clicked!') }"
    })
}

//Does an Ajax postback
///////
DoPostback = function (t, a, internalpostback)
{
    var s = _zSupervisor.getSupervisor();
    s.formSubmit = true;
    s.formTarget = t;
    s.formAction = a;
    s.internalpostback = internalpostback;
    Loading(true);
};

executeASPNETPostback = function (target, argument, href, old__doPostBack)
{
    var uframeid = GetContainingUFrame(target);
    var uframe = UFrameManager.getUFrame(uframeid);
    href = uframe.config.loadFrom;
    var form = GetFormByName("MainForm");
    form.__EVENTTARGET.value = unescape(target);
    form.__EVENTARGUMENT.value = unescape(argument);
    var formdata = GetAllHiddenFields(form);
    UFrameManager.submitForm(form, formdata, uframeid);
    return true;
};

//Inherits an object
function inherit(p)
{
    if (p == null) throw TypeError(); // p must be a non-null object
    if (Object.create) // If Object.create() is defined...
        return Object.create(p); // then just use it.
    var t = typeof p; // Otherwise do some more type checking
    if (t !== "object" && t !== "function") throw TypeError();
    function f() { }; // Define a dummy constructor function.
    f.prototype = p; // Set its prototype property to p.
    return new f(); // Use f() to create an "heir" of p.
}

//Gets the first named div in the parent chain of the current element
function getWidgetDiv(element)
{
    var current = element;
    while (current)
    {
        if (current.tagName == "DIV" && current.id && current.className == "zWidget")
        {
            return current;
        }
        current = current.parentNode;
    }
}

function getPageObjectByFrame(frameID)
{
    var s = _zSupervisor.getSupervisor();
    var pn = "u$" + frameID + "$p";
    if (!s[pn])
    {
        s[pn] = {};
    }
    return s[pn];
}

function getPageObject(element)
{
    return getPageObjectByFrame(GetContainingUFrame(element.id));
}

function getZWidget(element)
{
    var p = getPageObject(element);
    var d = getWidgetDiv(element);
    var dn = "div_" + d.id;
    if (!p[dn])
        p[dn] = {};
    return p[dn];
}


var currentUFrame;

//Get HREF
GetContainingUFrame = function (elementName)
{
    var current = document.getElementById(elementName);
    if (!current)
        current = document.getElementsByName(elementName)[0];
    while (current)
    {
        if (current.tagName && current.tagName == "DIV")
        {
            if (current.getAttribute("src"))
            {
                return current.getAttribute("id");
            }
        }
        current = current.parentNode;
    }
};

GetFormByName = function (name)
{
    var theForm = document.forms['MainForm'];
    if (!theForm)
    {
        theForm = document.MainForm;
    }
    return theForm;
};

GetAllHiddenFields = function (form)
{
    var hiddenlist = {};

    var elem = form.elements;
    //var opt = form.documents.options;
    for (var i = 0; i < elem.length; i++)
    {
        if (elem[i].tagName == "INPUT" || elem[i].tagName == "TEXTAREA")
        {
            if (elem[i].type == "radio")
            {
                if (getCheckedRadioValue(elem[i]))
                {
                    hiddenlist[elem[i].name] = getCheckedRadioValue(elem[i]);
                }
            }
            else
            {
                hiddenlist[elem[i].name] = elem[i].value;
            }
        }
    }
    return hiddenlist;
};

//Hook function for __doPostBack
//http://stackoverflow.com/questions/1230573/how-to-capture-submit-event-using-jquery-in-an-asp-net-application
hookPostBack = function ()
{
    //Override __doPostBack
    if (!MainFrameExists())
    {
        return false;
    }
    if (typeof __doPostBack != 'function')
    {
        var __doPostBack = function (t, a)
        {
            var theForm = document.forms['MainForm'];
            if (!theForm.onsubmit || (theForm.onsubmit() != false))
            {
                theForm.__EVENTTARGET.value = eventTarget;
                theForm.__EVENTARGUMENT.value = eventArgument;
                theForm.submit();
            }
        };
    }
    var old__doPostBack = __doPostBack;
    addToPostBack(function (t, a)
    {
        return DoPostback(t, a, old__doPostBack);
    });
};

addToPostBack = function (func)
{
    var old__doPostBack = __doPostBack;
    if (typeof __doPostBack != 'function')
    {
        __doPostBack = func;
    } else
    {
        __doPostBack = function (t, a)
        {
            func(t, a);
        }
    }
};

popStateHandler = function (ev)
{
    var uframe = UFrameManager.getUFrame(currentUFrame);
    uframe.navigate(location.href, false);
};


var zDiv = Class.extend({
    hide: function()
    {
        this.div.style.visibility = "hidden";
        this.div.style.display = "none";
        //console.log(this.divstring + ".hide()");
    /*
        if (this.shown)
        {
            this.shown = false;
            this.divhtmlswap = this.div.innerHTML;
            this.div.innerHTML = "";
            this.div.style.visibility = "hidden";
        }
        else
        {
        alert("already hidden: " + this.divstring);
        }
      */  
        //document.getElementById(this.div.id).style.visibility = "hidden";
    },

    show: function()
    {
        this.div.style.visibility = "visible";
        this.div.style.display = "block";
        //console.log(this.divstring + ".show()");
        /*
        if (!this.shown)
        {
            this.shown = true;
            this.div.innerHTML = this.divhtmlswap;
            this.divhtmlswap = "";
            this.div.style.visibility = "visible";
        }
        else
        {
            alert("already shown: " + this.divstring);
        }
        */
       // document.getElementById(this.div.id).style.visibility = "visible";
    },

    init: function(divObject, divString)
    {
        this.div = divObject;
        this.divstring = divString;
        //if (!this.div)
          //  window['test16'] = this;
        //alert("div init ( " + divObject + " , " + divString + ")");
        if (this.div.style.visibility == "hidden" || this.div.style.display == "none")
        {
            this.hide();
        }
    },
});

var zWidget = Class.extend({

    divs: {},

    //Builds the hierarchy tree.  Only named divs will be processed.   
    findNestedDivs: function (parentDivInternalString)
    {
        var currDiv, currName;

        //Set up initial variables
        currDiv = document.getElementById(this.divClientID); //Current div object to iterate on
         //Current node to add items to
        var nestList;

        //Resolve zDiv handle
        currName = this.divs;
        if (!parentDivInternalString)
        {
            nestList = "";
            this.divs.divstring = "divs";
            this.divs.name = parentDivInternalString;
        }
        else
        {
            nestList = parentDivInternalString.split(".");
            //Resolve object handle for internal string (item1.item2.item3)
            for (var i = 1; i < nestList.length; i++) //nest = short item name
                currName = currName[nestList[i]];
        }

        /*
        if (currDiv.style.visibility == "hidden" || currDiv.style.display == "none")
            currName.hide();   
        else
            currName.show();
        */

        //Iterate through current div's child objects
        for (var i = 0; i < currName.div.children.length; i++)
        {
            var childElement = currName.div.children[i];
            if (!childElement.tagName)
                continue;
            if (childElement.tagName  == "DIV" && childElement.id && childElement.className != "zWidget")
            {
                var childShortName = "$" + this.getShortName(childElement.id);
                currName[childShortName] = new zDiv(childElement, currName.divstring + "." + childShortName);
                this.findNestedDivs(currName[childShortName].divstring);
            }
            else if (childElement.className == "zElement")
            {
                var childShortName = "$" + this.getShortName(childElement.id);
                currName[childShortName] = new zDiv(childElement, currName.divstring + "." + childShortName);
                this.findNestedDivs(currName[childShortName].divstring);
            }
        }
    },

    getShortName: function(divClientID)
    {
        var suffix = divClientID.substring(this.divIDPrefix.length);
        return suffix;
    },

    getNestedDivs: function (parentDivShortID, childDivShortID)
    {
        for(var child in document.getElementById(parentDivShortID).children)
        {
            if (child.tagName == "DIV" && child.id == childDivShortID)
                return child;
        }
    },

    init: function (divShortID, divClientID)
    {
        this.divShortID = divShortID;
        this.divClientID = divClientID;
        this.divIDPrefix = divClientID.substring(0, (divClientID.length - divShortID.length)); //Gets the prefix of the div id in this widget
        this.divs = new zDiv(document.getElementById(divClientID), "divs");
        this.findNestedDivs(null);
    },
});

dataView = zWidget.extend({
    init: function(divShortID, divClientID)
    {
        this._super(divShortID, divClientID); //Constructor chain
    },
  

    resetView: function()
    {
        this.divs.$grid.hide();
        this.divs.$intro.show();
    },

    showGrid: function()
    {
        this.divs.$grid.show();
        this.divs.$intro.hide();
    },
});

EditableFragmentControl = zWidget.extend({
    init: function(divShortID, divClientID)
    {
        this._super(divShortID, divClientID); //Constructor chain
        this.cancelEdit();

        window['test1'] = this.divs.$edit;
        window['test2'] = this.divs.$view;
    },

    cancelEdit: function()
    {
        //this.divs.$view.$reader.div.innerText = this.divs.$edit.$editor.div.value.replace("\n");

        this.divs.$view.show();
        this.divs.$edit.hide();
    },

    edit: function()
    {
        this.divs.$edit.$editor.div.value = this.divs.$view.$reader.div.innerText.replace("<br>", "\n");
        
        this.divs.$view.hide();
        this.divs.$edit.show();
    },
});

goalConnections = zWidget.extend({
    init: function(divShortID, divClientID)
    {
        this._super(divShortID, divClientID); //Constructor chain
        this.resetView();
    },

    resetView: function()
    {
        this.divs.$grid.hide();
        this.divs.$showConnections.show();
    },

    showGrid: function()
    {
        this.divs.$grid.show();
        this.divs.$showConnections.hide();
    },
});

goalSelector = zWidget.extend({
    init: function(divShortID, divClientID, inputID)
    {
        this._super(divShortID, divClientID); //Constructor chain
        if (inputID)
            this.inputID = inputID;
        this.resetView();
    },
  

    resetView: function()
    {
        this.selectedGoal = "-1";
        document.getElementById(this.inputID).value = "-1";

        var elem = this.divs.$select
        
        elem.hide();
        //elem.style.visibility = "hidden";

        elem = this.divs.$final
        elem.hide();
        //elem.style.visibility = "hidden";

        elem = this.divs.$intro
        elem.show();
        //elem.style.visibility = "visible";

    },

    selectGoal: function (id)
    {
        this.selectedGoal = id;

        var elem = this.divs.$select;
        elem.hide();
        //elem.style.visibility = "hidden";

        elem = this.divs.$final;
        elem.show();
        //elem.style.visibility = "visible";

        elem = this.divs.$final.$result.div;
        elem.innerHTML = "Selected Goal: " + this.selectedGoal;

        if (id)
            document.getElementById(this.inputID).value = id;
        
    },

    selectGoalRequest: function()
    {
        var elem = this.divs.$intro;
        elem.hide();
        //elem.style.visibility = "hidden";

        elem = this.divs.$select;
        elem.show();
        //elem.style.visibility = "visible";
    }
});

goalView = zWidget.extend({
    init: function(divShortID, divClientID)
    {
        this._super(divShortID, divClientID); //Constructor chain
        this.resetView();
    },
  

    resetView: function()
    {
    },
});

newGoal = zWidget.extend({
    init: function(divShortID, divClientID)
    {
        this._super(divShortID, divClientID); //Constructor chain
    },

    resetView: function()
    {
    },
});



// return the value of the radio button that is checked
// return an empty string if none are checked, or
// there are no radio buttons
function getCheckedRadioValue(radioObj) {
	if(!radioObj)
		return "";
	var radioLength = radioObj.length;
	if(radioLength == undefined)
		if(radioObj.checked)
			return radioObj.value;
		else
			return "";
	for(var i = 0; i < radioLength; i++) {
		if(radioObj[i].checked) {
			return radioObj[i].value;
		}
	}
	return "";
}

// set the radio button with the given value as being checked
// do nothing if there are no radio buttons
// if the given value does not exist, all the radio buttons
// are reset to unchecked
function setCheckedRadioValue(radioObj, newValue) {
	if(!radioObj)
		return;
	var radioLength = radioObj.length;
	if(radioLength == undefined) {
		radioObj.checked = (radioObj.value == newValue.toString());
		return;
	}
	for(var i = 0; i < radioLength; i++) {
		radioObj[i].checked = false;
		if(radioObj[i].value == newValue.toString()) {
			radioObj[i].checked = true;
		}
	}
}