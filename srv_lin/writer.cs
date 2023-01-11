using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Serilog;
using System.Text.Json;

namespace srv_lin;
public class CWriter : IDisposable
{
    private IConnection? m_connection;
    private IModel? m_channel;
    private string m_str_exch = "";

    public CWriter()
    {
    }

    ~CWriter() 
    {
        // not called??
        Log.Information("Writer.Dispose!");
        Dispose();
    }

    public void Dispose() 
    {
        Log.Information("Writer.Dispose22222!");
        m_channel?.Abort();
        m_channel?.Dispose();
        m_channel = null;
        m_connection?.Abort();
        m_connection?.Dispose();
        m_connection = null;
    }

    class CWriterException: Exception
    {

    }

    public void Init( Worker.CParams par, Microsoft.Extensions.Logging.ILogger _logger )
    {
        try
        {
            Log.Information("init Writer!");
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName    = par.m_str_host;
            factory.Port        = par.m_n_port;
            factory.VirtualHost = "/";
            factory.UserName    = par.m_str_user; // guest - resctricted to local only
            factory.Password    = par.m_str_pass;
            _logger.LogWarning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
            Log.Warning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
            m_connection = factory.CreateConnection();
            m_channel    = m_connection.CreateModel();
            if( (par.m_str_exch_events       == null) ||
                (par.m_str_exch_events.Length < 1   )
            )
            {
                throw  new Exception("par.m_str_exch_events -> error!");
            }
            m_str_exch = par.m_str_exch_events;
            Log.Warning($"EXCHANGE-> [{m_str_exch}]");
            m_channel.ExchangeDeclare( exchange: m_str_exch, type: ExchangeType.Fanout, durable: false, autoDelete:true );
        }
        catch(Exception ex)
        {
            Dispose();
            Log.Error($"Exception: {ex.Message}");
            throw;
        }
        Log.Information("init Writer.finished()!");
    }

    public int Publish(string strMsg)
    {
        try
        {
            if(m_channel==null)
                return -1;
            if(m_str_exch.Length < 1)
                return -2;
            byte[] body = Encoding.UTF8.GetBytes(strMsg);
            Log.Information($"publish to [{m_str_exch}] : [{strMsg}]");
            Task ttt = Task.Run(()=>{m_channel.BasicPublish( exchange: m_str_exch, routingKey: "", basicProperties: null, body: body );});
            //bool b = ttt.Wait(1000);
            //m_channel.BasicPublish( exchange: m_str_exch, routingKey: "", basicProperties: null, body: body );
        }
        catch(Exception ex)
        {
            return -3;
        }
        return 1;
    }
/*
    public static int ThreadListen( CParams par, Microsoft.Extensions.Logging.ILogger _logger, OnCommand on_command )
    {
        int nRes = 0;
        Log.Information("start listen!");
        ConnectionFactory factory = new ConnectionFactory();
        factory.HostName    = par.m_str_host;
        factory.Port        = par.m_n_port;
        factory.VirtualHost = "/";
        factory.UserName    = par.m_str_user; // guest - resctricted to local only
        factory.Password    = par.m_str_pass;
        _logger.LogWarning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
        Log.Warning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare( exchange: par.m_str_exch_commands, type: ExchangeType.Fanout, durable: false, autoDelete:true );
            var queue_name = channel.QueueDeclare().QueueName;
            channel.QueueBind( queue: queue_name, exchange: par.m_str_exch_commands, routingKey: "" );
            Log.Information($"Waiting for commands queue [{queue_name}]");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ( model, ea ) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Log.Information($"get message: {message}");
                CommandSerialized? command_serialized = null;
                try
                {
                    command_serialized = JsonSerializer.Deserialize<CommandSerialized>(message)!;
                }
                catch(Exception ex)
                {
                    Log.Error($"exception -> [{ex.Message}] when trying deserialize command [{message}]");
                    command_serialized = null;    
                }   
                Command command = new Command(command_serialized);
                nRes = on_command(command);
                Log.Information($"command ret: {nRes}");
            };
            channel.BasicConsume( queue: queue_name, autoAck: true, consumer: consumer );
            par.m_cncl_tkn.WaitHandle.WaitOne();
            Log.Warning($"listener get cancel signal.");
        }
        return 1;
    }
*/    
}
