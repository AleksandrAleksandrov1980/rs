
      const g_enCOMMDANDS = Object.freeze({
        STATE            : "STATE",
        PROC_RUN         : "PROC_RUN",
        PROC_EXTERMINATE : "PROC_EXTERMINATE",
        GRAM_START       : "GRAM_START",
        GRAM_STOP        : "GRAM_STOP",
        GRAM_KIT         : "GRAM_KIT",
        GRAM_STATE       : "GRAM_STATE",
        FILE_UPLOAD      : "FILE_UPLOAD",
        FILE_DOWNLOAD    : "FILE_DOWNLOAD",
        DIR_MAKE         : "DIR_MAKE",
      });

      document.addEventListener('DOMContentLoaded', function() {
        let el = document.getElementById("path_to_dir_mk");
        //el.value = "/CALCS/2023_2_4___7_18_5_997/"; 
        el.value = "/CALCS/"+GetTmStamp()+"/"; //!!!! timeStamp
        document.getElementById("path_to_dir_upload").value = el.value;
        //document.getElementById("path_to_file_download").value = el.value + "/results.xml";
        document.getElementById("path_to_file_download").value = el.value + "/res";
        //<input type="text" id="path_to_file_exec" size="30" value="C:/projects13/RastrWin/main/RastrWin/Release/master.exe"> 
        //<input type="text" id="file_exec_params" size="30" value="-j222 D:/Vms/SHARA/crosses/IA/2023_02_01_file2/calc//s1.log"> 
        document.getElementById("path_to_file_exec").value = "/home/ustas/projects/git_r/Astral/build/astra";
        document.getElementById("file_exec_params").value = "-se /var/rs_wrk/CALCS/2023_2_4___7_18_5_997/roc_debug_from_SQL_2.os";
      }, false);
    
      class CStompHlp
      {

        constructor(name_exchng_in, id_chnl_state_in, id_collapse_card_in, id_t){
          this.name_exchng         = name_exchng_in;
          this.id_chnl_state       = id_chnl_state_in;
          this.id_collapseCard     = id_collapse_card_in;
          this._nCounterMsgs       = 0;
          this._nCounterLogRecords = 0;
          this._n_counter_sended_msgs = 0;
          this._arr_objs           = [];
          this._id_t               = id_t;
        }

        static log(str_log){
          console.log( '['+this.name_exchng + ']: ' + str_log);
        }

        table_populate(){
          let table = document.querySelector(this._id_t);
          if(table!=null){
            if(this._arr_objs.length>0){
              if ( $.fn.dataTable.isDataTable( this._id_t ) ) {
                let table = $(this._id_t).DataTable();
                table.clear(); // Clear your data
                table.rows.add(this._arr_objs); // Add rows with newly updated data
                table.draw(); //then draw it
              }else{
                let columns = [];
                let id_col_num = -1;
                let i = 0;
                for (let key of Object.keys(this._arr_objs[0])) {
                  if(key==="num")
                    id_col_num = i;
                  columns.push({ data: key, title: key} );
                  i++;
                }	
                $(this._id_t).DataTable( { // Table.Initialization!
                  data: this._arr_objs,  
                  columns,
                  serverSide: false,
                  "order": [[ id_col_num, 'asc' ]]
                } );
              }
            }
          }else{
            CStompHlp.log('no table: ' + this._id_t);
          }
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
              this.table_populate();
            } else {
              CStompHlp.log('got empty message');
            }
          }catch(err){
            CStompHlp.log('catched exception!: ' + err);
            this.AddLog( "< " + this._nCounterMsgs.toString()+ "  error parse! : " + message.body );
          }
        };

        AddLog(str_log) {
          let collapseCard = document.querySelector(this.id_collapseCard);
          if( this._nCounterLogRecords++ > 30 ){ 
            collapseCard.innerText = "";
            this._nCounterLogRecords = 0;
          }
          collapseCard.innerHTML += str_log.toString()+ "<BR/>";
        }

        SetChnlState(new_state)
        {
          let elem = document.getElementById(this.id_chnl_state);
          if(elem != null){
            if(new_state===true){
              elem.style.background = 'green';
            }else{
              elem.style.background = 'red';
            }
          }
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
                CStompHlp.log(str);
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

        SendCommand(en_command, str_params){
          let strData = `> ${en_command} : '${str_params}'`;
          let bl_error = true;
          for(let en in g_enCOMMDANDS) {
            if(en === en_command){
              bl_error = false;
              break;
            }
          }
          if(bl_error === true){
            alert(`error. get unknown command ${strData}`);  
            return;
          }
          // trying to publish a message when the broker is not connected will throw an exception
          if(!this.client.connected){
            alert("Broker disconnected, can't send message.Try again lette!");
            return ;
          }
          this._n_counter_sended_msgs++;
         
          let command = new CCommand();
          command.command = en_command;
          command.for = "xz";
          command.from = "web_s";
          command.sign = "unsiggned";
          let el = null;
          let payLoad = null;
          if      (en_command===g_enCOMMDANDS.STATE){
          }else if(en_command===g_enCOMMDANDS.DIR_MAKE){
            command.pars = [ document.getElementById("path_to_dir_mk").value] ;
          }else if(en_command===g_enCOMMDANDS.FILE_UPLOAD){            
            command.pars = [ document.getElementById("path_to_file_upload").value, document.getElementById("path_to_dir_upload").value ];   
          }else if(en_command===g_enCOMMDANDS.PROC_RUN){
            command.pars = [ document.getElementById("path_to_file_exec").value, document.getElementById("file_exec_params").value ];  
          }else if(en_command===g_enCOMMDANDS.PROC_EXTERMINATE){
            command.pars = [ document.getElementById("proc_term").value ];  
          }else if(en_command===g_enCOMMDANDS.FILE_DOWNLOAD){
            command.pars = [ document.getElementById("path_to_file_download").value, document.getElementById("path_to_dir_download").value ];   
          }else if(en_command===g_enCOMMDANDS.GRAM_START){
          }else if(en_command===g_enCOMMDANDS.GRAM_STATE){
          }else if(en_command===g_enCOMMDANDS.GRAM_KIT){
          }else if(en_command===g_enCOMMDANDS.GRAM_STOP){
          }else{
            alert("Unknown command! " + en_command )
          }
          payLoad = JSON.stringify(command);
          this.client.publish({destination: '/exchange/rs_commands', body: payLoad });
        }

      };// class CStompHlp

      function GetTmStamp(){
        let currentdate = new Date(); 
        let datetime = currentdate.getFullYear()  + "_"
                    + (currentdate.getMonth()+1)  + "_" 
                    +  currentdate.getDate()      + "___"  
                    +  currentdate.getHours()     + "_"  
                    +  currentdate.getMinutes()   + "_" 
                    +  currentdate.getSeconds()   + "_"
                    +  currentdate.getMilliseconds();
        return datetime;
      }

      class CCommand{
        constructor(){
          this.command = null;
          this.for = "";
          this.from = "";
          this.tm_mark = GetTmStamp();
          this.pars = [];
          this.sign = "";
        }
      };

      var g_sh_events   = new CStompHlp( '/exchange/rs_events', 'cb_exc_evts_state', '#events_log', '#table_events' );
      g_sh_events.connect();
      var g_sh_commands = new CStompHlp( '/exchange/rs_commands', 'cb_exc_cmds_state', '#commands_log', '#table_commands' );
      g_sh_commands.connect(); 

      function UpdateDirUpload(){
        let el_path_to_dir_mk = document.getElementById("path_to_dir_mk");
        let el_path_to_dir_upload = document.getElementById("path_to_dir_upload");
        el_path_to_dir_upload.value = el_path_to_dir_mk.value; 
        document.getElementById("path_to_file_download").value = document.getElementById("path_to_dir_mk").value + "/res"
      }
       
      function CheckFtp(){
        try {
          var xhr = new XMLHttpRequest();
          xhr.open("PUT", "ftp://anon:anon@192.168.1.59/PlainRastr.cpp", true);
          xhr.setRequestHeader('Content-Type', 'text/plain');
          xhr.send('hrlo');
          console(xhr.responseText);
        }
        catch(err) {
          console( "exc: "+ err.message);
        }
      }

      async function Sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
      }

      async function PlayScn1(){
        let delay = 1000;

        await Sleep(delay);
        g_sh_commands.SendCommand(g_enCOMMDANDS.STATE,"");
        await Sleep(delay);
        g_sh_commands.SendCommand(g_enCOMMDANDS.DIR_MAKE,"");
        await Sleep(delay);
        g_sh_commands.SendCommand(g_enCOMMDANDS.FILE_UPLOAD,"");
        await Sleep(delay);
        g_sh_commands.SendCommand(g_enCOMMDANDS.PROC_RUN,"");
        await Sleep(delay);
        g_sh_commands.SendCommand(g_enCOMMDANDS.PROC_EXTERMINATE,"");
        await Sleep(delay);
        g_sh_commands.SendCommand(g_enCOMMDANDS.FILE_DOWNLOAD,"");
        await Sleep(delay);
        g_sh_commands.SendCommand(g_enCOMMDANDS.STATE,"");
        await Sleep(delay);

        await Sleep(delay);
      }

      var g_prms_cycle_scn1 = undefined;
      var g_bl_cycle_scn1_exit = false;
      async function AsyncLunchCycleScn1(){
        if(g_prms_cycle_scn1 !== undefined)
          return;
        return  new Promise( async p1 => {
          for(;;)
          {
            PlayScn1();
            await Sleep(1000);
            if(g_bl_cycle_scn1_exit == true){
              g_bl_cycle_scn1_exit = false;
              g_prms_cycle_scn1 = undefined;
              break;
            }
          }
        });
      }

      function AsyncStopCycleScn1(){
        g_bl_cycle_scn1_exit = true;
      }
   
    