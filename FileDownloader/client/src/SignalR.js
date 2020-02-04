import React, { PureComponent } from "react";

import * as signalR from "@aspnet/signalr";

class SignalR extends PureComponent {
  state = { message: "" };
  constructor(props) {
    super(props);

    this.connection = null;
    this.onNotifReceived = this.onNotifReceived.bind(this);
  }

  componentDidMount() {
    const protocol = new signalR.JsonHubProtocol();

    const transport = signalR.HttpTransportType.WebSockets;

    const options = {
      transport,
      logMessageContent: true,
      logger: signalR.LogLevel.Trace,
      accessTokenFactory: () => this.props.accessToken
    };

    // create the connection instance
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:1000/chatHub", options)
      .withHubProtocol(protocol)
      .build();

    this.connection.on("ReceiveMessage", this.onNotifReceived);

    this.connection
      .start()
      .then(() => console.info("SignalR Connected"))
      .catch(err => console.error("SignalR Connection Error: ", err));
  }

  componentWillUnmount() {
    this.connection.stop();
  }

  onNotifReceived(res) {
    console.log("Yayyyyy, I just received a notification!!!", res);
    this.setState({ message: res });
  }

  render() {
    return <span>{this.state.message}</span>;
  }
}

export default SignalR;
