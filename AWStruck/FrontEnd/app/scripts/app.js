/** @jsx React.DOM */

var mountNode = document.getElementById("app");


var dummyData = [
    { name: "Environment 1", id: "Env1", isOn: true, startAt: "07:00", stopAt: "19:00" },
    { name: "Environment 2", id: "Env2", isOn: false, startAt: "07:00", stopAt: "20:00" }
];

var connection = $.hubConnection("http://54.149.71.156/awstruck");
var proxy = connection.createHubProxy('switchHub');

var isOn = function( env ) {
    return env.State === "running"
      ? true
      : false;
}

var getData = function( env ) {
    console.log( env.State )
    return {
        name: env.Name,
        isOn: isOn( env ),
        startAt: "07:00",
        stopAt: "20:00"
    }
}

proxy.on('signal', function( data ) {

    if( data.length ) {
        dummyData = data.map( getData );
        React.renderComponent(<App />, mountNode);
    }
})



connection.start()
    .done(function(){ console.log('Now connected, connection ID=' + connection.id, arguments); })
    .fail(function(){ console.log('Could not connect'); });


var App = React.createClass({
  getInitialState: function() {
    return { items: dummyData };
  },
  render: function() {
    return (
      <div>
        <div className='container-fluid' >
          <EnvList items={this.state.items} />
        </div>
      </div>
    );
  }
});


