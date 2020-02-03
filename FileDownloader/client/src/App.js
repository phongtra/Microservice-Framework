import React, { useState, useEffect } from "react";
import axios from "axios";

import SignalR_Func from "./SignalR_Func";

const App = () => {
  const [button, setButton] = useState(false);
  const onButtonClick = async () => {
    await axios.post("http://localhost:1200/values", {
      value:
        "https://drive.google.com/uc?export=download&id=1AKZTeum65Fqwal95lCGf3TirjNMyJz5q"
    });
    setButton(!button);
  };
  return (
    <div>
      <button onClick={onButtonClick}>Press to trigger</button>
      <SignalR_Func />
    </div>
  );
};

export default App;
