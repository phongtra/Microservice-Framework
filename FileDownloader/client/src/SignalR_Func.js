import React, { useEffect, useState } from "react";

import * as signalR from "@aspnet/signalr";

const SignalR_Func = props => {
  const [message, setMessage] = useState("");
  useEffect(() => {
    const protocol = new signalR.JsonHubProtocol();

    const transport = signalR.HttpTransportType.WebSockets;

    const options = {
      transport,
      logMessageContent: true,
      logger: signalR.LogLevel.Trace,
      accessTokenFactory: () => props.accessToken
    };

    // create the connection instance
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:1000/chatHub", options)
      .withHubProtocol(protocol)
      .build();

    connection.on("ReceiveMessage", res => {
      console.log(res);
      setMessage(res);
    });

    connection
      .start()
      .then(() => console.info("SignalR Connected"))
      .catch(err => console.error("SignalR Connection Error: ", err));
    return () => connection.stop();
  }, []);
  return <div>{message}</div>;
};

export default SignalR_Func;
