<home>

    <p>Seconds Elapsed: { time }</p>
    <p>Beats: { state.beats }</p>
    <div>
        <a 	onclick={this.registerHeartBeat}  
            class={this.registrationResult == null?'btn btn-default btn-lg':'disabled btn btn-default btn-lg'}>
            Register HeartBeat</a>

        <a 	onclick={this.unregisterHeartBeat} 
            class={this.registrationResult != null?'btn btn-default btn-lg':'disabled btn btn-default btn-lg'}>
            Unregister HeartBeat</a>
    
    </div>

<script>
  var self = this;
  self.state = {
      beats:0
  }
  self.registrationResult = null;
  self.heartBeat = {
        heart: (data, callback) => {
            var hb = self.heartBeat;
            console.log(data);
            callback(null, data.key);
            ++self.state.beats;
        }
    }

    self.time = opts.start || 0

    tick() {
      self.update({ time: ++self.time })
    }

    var timer = setInterval(self.tick, 1000)

    self.on('unmount', function() {
      clearInterval(timer)
    })

    self.registerHeartBeat = () => {
        if(self.registrationResult == null){
            window.boundAsync.localFetch('local://v1/command-source/register-heart',
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'X-Symc-Fetch-App-Version': '1.0'
                    },
                    body: self.heartBeat
                }, function(error, result) {
                    if (error) throw error;
                    console.log(result);
                    self.registrationResult = result;
                });
        }
  	};

    self.unregisterHeartBeat = () => {
        if(self.registrationResult != null){
            window.boundAsync.localFetch('local://v1/command-source/unregister-heart',
                {
                
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Symc-Fetch-App-Version': '1.0'
                },
                body: {key:self.registrationResult.value.key}
            }, function(error, result) {
                if (error) throw error;
                console.log(result);
                self.registrationResult = null;
            });
        }
  	};
  </script>

</home>