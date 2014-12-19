/** @jsx React.DOM */

var Switch = React.createClass({

    render: function() {
      return (
        <div className='switch'>
            <div className='switch-slider'>
                <div className='switch-label'>On</div>
                <div className='switch-thumb-container'>
                    <div className='visible-thumb'>
                    </div>
                </div>
                <div className='switch-label off'>Off</div>
            </div>
        </div>
      )
    }
});
