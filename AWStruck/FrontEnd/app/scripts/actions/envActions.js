var envDispatcher = new Dispatcher();

EnvActions = {

    start: function( name ) {
        envDispatcher.dispatch( { actionType: 'on', env: name } )
    },
    end: function( name ) {
        envDispatcher.dispatch( { actionType: 'off', env: name } )
    }
}
