import React, { useState, useEffect } from "react";
import axios from "axios";

const App = () => {
  const [button, setButton] = useState(false);
  const onButtonClick = async () => {
    await axios.post("http://localhost:1200/values", {
      value: "http://releases.ubuntu.com/18.04/ubuntu-18.04.3-desktop-amd64.iso"
    });
    setButton(!button);
  };
  return (
    <div>
      <button onClick={onButtonClick}>Press to trigger</button>
    </div>
  );
};

export default App;
