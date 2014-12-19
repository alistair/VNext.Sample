
var sendMessage = function( connectionName, envId ) {

    console.log( connectionName, envId )

    proxy.invoke( connectionName, envId ).done(function() {
        console.log('success', arguments);
    }).fail(function() {
        //console.log('fail', arguments)
    })
}


var envAction = function( payload ){

console.log(payload)

    if( payload.actionType === "off" ) {
        sendMessage("stopEnv", "TestEnv2");
    } else {
        sendMessage("startEnv", "TestEnv2");
    }

}

envDispatcher.register( envAction )
