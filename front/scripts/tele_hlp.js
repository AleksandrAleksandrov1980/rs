


class CStompTele
{
  
  constructor(){
    this.name_exchng         = "rastr_tele";
  }

  log(str_log){
     console.log( '['+this.name_exchng + ']: ' + str_log);
  }

  callback_subscribe (message){
    try{
      if (message.body){
        this.log('got msg with body : ' + message.body);
        let obj_msg = JSON.parse(message.body);   
        //d3.select("#node_1690_vras").text( Math.floor(Math.random() * 10000000)); // WORKED!!!
        //d3.select("#node_1690_vras").text( Math.floor(Math.random() * 10000000)); // WORKED!!!
        for(let hlp of obj_msg){ 
          let s = d3.select("#"+hlp.str_id);
          s.text(hlp.str_val+ Math.floor(Math.random() * 100));
        }
      } else {
          this.log('got empty message'); 
      }
    }catch(err){
      this.log('callback_subscribe() exception: ' + err);
    }
  };

  connect(){
    //brokerURL : 'ws://192.168.1.59:15674/ws',
    //brokerURL : 'ws://alexandrov-7k:15674/ws',
    this.client = new StompJs.Client({
      brokerURL: 'ws://alexandrov-7k:15674/ws',
      connectHeaders: {
        login    : 'rastr',
        passcode : 'rastr',
      },
      reconnectDelay:    5000,
      heartbeatIncoming: 10000,
      heartbeatOutgoing: 10000,
      debug: function (str){
          console.log('debug: '+str);
      },
    });

    this.log('connected to: '+this.client.brokerURL);

    this.client.onConnect = function (frame) {
      // Do something, all subscribes must be done is this callback
      // This is needed because this will be executed after a (re)connect
      this.log('onConnect: trying subscribe to ' + this.name_exchng + ' <> ' + this.callback_subscribe );
      //when subscription closed, websocket already closed!!! and reopend in cycle!!
      this.subscription_events = this.client.subscribe( this.name_exchng, this.callback_subscribe.bind(this) );
    };
    this.client.onConnect = this.client.onConnect.bind(this);
    this.client.onDisconnect = (frame) => {
      this.log('onDisconnect : ' + frame.body);
    }
    this.client.onDisconnect = this.client.onDisconnect.bind(this);
    this.client.onStompError = function (frame) {
      // Will be invoked in case of error encountered at Broker
      // Bad login/passcode typically will cause an error
      // Complaint brokers will set `message` header with a brief message. Body may contain details.
      // Compliant brokers will terminate the connection after any error
      this.log('onStompError: ' + frame.headers['message']);
      this.log('onStompError Additional details: ' + frame.body);
    };
    this.client.onWebSocketClose = function (evt)  {
      this.log( 'onWebSocketClose: ' + evt );
    };
    this.client.onWebSocketClose = this.client.onWebSocketClose.bind(this)
    this.client.onUnhandledMessage = function (message) {
      this.log( 'onUnhandledMessage: message = ' + message );
    };
    this.client.onUnhandledReceipt = function (frame)  {
      this.log('onUnhandledReceipt: ' + frame.body);
    };
    this.client.onUnhandledFrame  = function (frame)  {
      this.log('onUnhandledFrame: ' + frame.body);
    };
    this.client.activate();
  };

};// class CStompTele

var g_StompTele   = new CStompTele();
g_StompTele.connect();

   
//alert("fwefe");