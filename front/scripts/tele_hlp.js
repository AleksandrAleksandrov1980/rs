


class CStompTele
{

  constructor(){
    this.name_exchng         = "rastr_tele";
 }

 static log(str_log){
   console.log( '['+this.name_exchng + ']: ' + str_log);
 }


  callback_subscribe (message){
    try{
      if (message.body){
        CStompHlp.log('got msg with body : ' + message.body);
        this._nCounterMsgs++;
        //this.AddLog( "< " + this._nCounterMsgs.toString()+ " : " + message.body );
        let obj_msg = JSON.parse(message.body);
        obj_msg.num = this._nCounterMsgs;
        this._arr_objs.push(obj_msg);
       } else {
          CStompHlp.log('got empty message'); 
      }
    }catch(err){
      CStompHlp.log('catched exception!: ' + err);
      this.AddLog( "< " + this._nCounterMsgs.toString()+ "  error parse! : " + message.body );
   }
  };

  AddLog(str_log) {
  }

  connect(){
    //brokerURL : 'ws://192.168.1.59:15674/ws',
    //brokerURL : 'ws://alexandrov-7k:15674/ws',
    this.client = new StompJs.Client({
      brokerURL: 'ws://alexandrov-7k:15674/ws',
      connectHeaders: {
        login    : 'rastr',
        passcode : 'rastr',
      },
      reconnectDelay:    1000,
      heartbeatIncoming: 4000,
      heartbeatOutgoing: 4000,
      debug: function (str){
        //CStompHlp.log(str);
          console.log('debug: '+str);
      },
    });

    this.client.onConnect = function (frame) {
      // Do something, all subscribes must be done is this callback
      // This is needed because this will be executed after a (re)connect
      CStompHlp.log('onConnect: trying subscribe to ' + this.name_exchng + ' <> ' + this.callback_subscribe );
      //when subscription closed, websocket already closed!!! and reopend in cycle!!
      this.subscription_events = this.client.subscribe( this.name_exchng, this.callback_subscribe.bind(this) );
      this.table_populate = this.table_populate.bind(this);
      this.SetChnlState(true);
    };
    this.client.onConnect = this.client.onConnect.bind(this);

    this.client.onDisconnect = (frame) => {
      CStompHlp.log('onDisconnect : ' + frame.body);
      this.SetChnlState(false);
    }
    this.client.onDisconnect = this.client.onDisconnect.bind(this);

    this.client.onStompError = function (frame) {
      // Will be invoked in case of error encountered at Broker
      // Bad login/passcode typically will cause an error
      // Complaint brokers will set `message` header with a brief message. Body may contain details.
      // Compliant brokers will terminate the connection after any error
      CStompHlp.log('onStompError: ' + frame.headers['message']);
      CStompHlp.log('onStompError Additional details: ' + frame.body);
    };

    this.client.onWebSocketClose = function (evt)  {
      CStompHlp.log( 'onWebSocketClose: ' + evt );
      this.SetChnlState(false);
    };
    this.client.onWebSocketClose = this.client.onWebSocketClose.bind(this)

    this.client.onUnhandledMessage = function (message) {
      CStompHlp.log( 'onUnhandledMessage: message = ' + message );
    };

    this.client.onUnhandledReceipt = function (frame)  {
      CStompHlp.log('onUnhandledReceipt: ' + frame.body);
    };

    this.client.onUnhandledFrame  = function (frame)  {
      CStompHlp.log('onUnhandledFrame: ' + frame.body);
    };

    this.client.activate();
  };

};// class CStompTele

var g_StompTele   = new CStompTele();
g_StompTele.connect();


/*
let client = new StompJs.Client({
              brokerURL: 'ws://alexandrov-7k:15674/ws',
              connectHeaders: {
                login    : 'rastr',
                passcode : 'rastr',
              },
              reconnectDelay:    1000,
              heartbeatIncoming: 4000,
              heartbeatOutgoing: 4000,
              debug: function (str){
                CStompHlp.log(str);
              },
            });

*/
    
alert("fwefe");