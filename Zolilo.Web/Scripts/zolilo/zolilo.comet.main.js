ZoliloCallback.prototype = {
    GetKeys: function (obj)
    {
        var keys = [];
        for (var i in obj)
        {
            keys.push(i);
        }
        return keys;
    },

    CompileZoliloViewState: function ()
    {
        //Load view state
        /*
        var viewStateString = "";

        for (var i in this.viewState)
        {
            viewStateString = viewStateString.concat(i + "☺" + this.viewState[i] + "☻");
        }
        document.getElementById("ZoliloViewState_hf").value = viewStateString;
        */
        //alert("CompileZoliloViewState = " + viewStateString);
        //document.getElementById("ZoliloViewState_hf").value = "VIEWSTATE SUCCESS";
    },

    UpdateZoliloViewState: function (key, value)
    {
        this.viewState[key] = value;
    },

    ExecuteCallbackServer: function ()
    {
        //CallServer('test', '');
    },




    //Polls the server for commands received
    Comet: function (args)
    {
        // StartCallServer("Comet", "poll");
    },

    _Comet: function (args)
    {
        //debugger;
        for (i = 0; i < args.length; i++)
        {
            var item = args[i];
            var servercmdid = item[0];
            var command = item[1];

            var result = eval(command);
            this.CometReturn(servercmdid, result);
        }
    },

    //Sends a return value to the server
    //servercmdid = the id assigned by the server to the comet statement
    //returnvalue = the return value of the function executed by the client
    //returns: nothing
    CometReturn: function (servercmdid, returnvalue)
    {
        this.StartCallServer("CometReturn", servercmdid + "◄" + returnvalue);
    },

    //Receives a response from the server after CometReturn
    _CometReturn: function (args)
    {
        if (args.length > 1)
        {
            args = args.substring(1);
            eval(args);
        }
    },


    //Timer functions


    //Returns true if the time since polling began is greater than maxTimeToShortPoll
    ShortPollingExpired: function ()
    {
        return (new Date().getTime() - this.timeShortPollBegan > this.maxTimeToShortPoll);
    },



    EndShortPolling: function ()
    {
        //alert("EndShortPolling");
        this.shortPollingEnabled = false;
    }
};

GetZCallback = function (uframeID)
{
    var suffix = "";
    if (!uframeID && uframeID.length > 0)
        suffix += ("_" + uframeID);
    var name = "zCallback" + suffix;
    return name;
};


    function ZoliloCallback(id)
    {
        this.id = id;

        //Set name of control
        var nameOfControl = GetZCallback(id);

        //Set suffix 
        var suffix = "";
        if (!id && id.length > 0)
            suffix += ("_" + id);
        this.nameSuffix = suffix;

        //Timer
        // if this is true, server will send a request to server at specified interval
        // do not edit directly, use BeginShortPolling() / EndShortPolling()
        this.shortPollingEnabled = false;

        this.globalTimer = window.setInterval(nameOfControl + ".GlobalTimerCallback(" + nameOfControl + ")", 250);
        //alert("Test()");

        this.viewState = {};
        this.counter1 = 0;
        this.callbackList = {};

        // The time, in ms, that must be "missed" before we
        // assume the app was put to sleep.
        this.timeSleepThreshold = 10000;

        // The length of time, in ms, that the client will continuously send requests to the server
        this.maxTimeToShortPoll = 0;

        // The specific time that the polling began
        this.timeShortPollBegan = 0;

        this.lastTick_ = 0;

        this.GlobalTimerCallback = function (callbackObject)
        {
            //alert(callbackObject);
            if (callbackObject.shortPollingEnabled == true)
            {
                if (callbackObject.ShortPollingExpired())
                {
                    callbackObject.EndShortPolling();
                }
                else
                {
                    //alert("start");
                    callbackObject.StartCallServer("Comet", "poll");
                }
            }
        };

        this.BeginShortPolling = function (time)
        {
            //alert("BeginShortPolling(" + time + ")");
            this.maxTimeToShortPoll = time;
            this.shortPollingEnabled = true;
            //alert(this.shortPollingEnabled);
            this.timeShortPollBegan = new Date().getTime();
        };



        this.ReceiveServerDataObj = function (rValue)
        {
            //alert("rValue:" + rValue);
            /*
            var callbackSplit = rValue.split("►►☼►☼☺☻♥☺");
            document.getElementById("ZoliloViewState_hf").value = callbackSplit[0];

            rValue = callbackSplit[1];
            */

            // alert("ReceiveServerData(" + rValue + ")");
            var index = rValue.indexOf("↨");
            if (index < 0)
                return;

            var valueArray = rValue.split("↨"); //17

            //First value is counter
            var counterValue = valueArray[0];
            //alert("counterValue: " + counterValue);
            //Rest of values are return values and optional parameters
            var callBackParams = valueArray[1];
            //alert("callBackParams: " + callBackParams);
            if (!this.callbackList[counterValue])
                throw "Error accessing callbackList - ServerData:" + rValue.toString();

            //Retrieve original command name from command hashtable
            var command = this.callbackList[counterValue];
            this.callbackList[counterValue] = null;

            //Turn params into array
            callBackParams = callBackParams.split("↨");

            //Call callback of function starting with _
            eval("this._" + command + "(" + callBackParams.toString() + ");");
        };

        //This function does not have access to the object's functions using the "this" modifier. It must get the ID via the rValue
        this.ReceiveServerData = function (rValue)
        {
            var index = rValue.indexOf("↨");
            if (index < 0)
                return;

            var valueArray = rValue.split("↨"); //17
            var callbackID = valueArray[0];
            var newrValue = rValue.substring(rValue.indexOf("↨") + 1);
            var call = callbackID + ".ReceiveServerDataObj(\"" + newrValue + "\")";
            //alert(call);
            eval(call);
            //this.ReceiveServerDataObj(rValue);
        };

        //Begins to send a request to the server
        //functionName (string) = the name of the function to invoke
        //args = the function arguments, separated with ◄
        this.StartCallServer = function (functionName, args)
        {
            //alert("StartCallServer: " + functionName + ", " + args + "; id = " + this.id);
            this.CompileZoliloViewState();
            this.callbackList[this.counter1] = functionName;
            var call = "CallServer" + this.nameSuffix + "('" + this.counter1 + "◄" + functionName + "◄" + args + "', '')";
            //alert("eval(" + call + ")");
            eval(call);
            this.counter1++;
        };



    }