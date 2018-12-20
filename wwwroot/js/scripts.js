
  // Initialize the platform object:
  var platform = new H.service.Platform({
      'app_id': '4xPg7L1BtEiY8niKAfrl',
      'app_code': '4507u1Z0vCXK5m02j6bnxQ'
      });
  
      // Get an object containing the default map layers:
      var defaultLayers = platform.createDefaultLayers();
  
      // Obtain the default map types from the platform object
      var maptypes = platform.createDefaultLayers();
  
      // Instantiate (and display) a map object:
      var map = new H.Map(
        document.getElementById('mapContainer'),
        maptypes.normal.map,
        {
          zoom: 11,
          center: { lng: -122.33, lat: 47.61 }
        }
      );
  
      // Store markers
      var icon = new H.map.Icon('/img/pin1.png');
  
      // Enable the event system on the map instance:
      var mapEvents = new H.mapevents.MapEvents(map);
  
      // Add event listener:
      map.addEventListener('tap', function(evt) {
        console.log(evt.type, evt.currentPointer.type); 
      });
      // Instantiate the default behavior, providing the mapEvents object: 
      var behavior = new H.mapevents.Behavior(mapEvents);
  
        // Change the map base layer to the satellite map with traffic information:
      map.setBaseLayer(defaultLayers.normal.map);
      var ui = H.ui.UI.createDefault(map, defaultLayers);
  
      //Changes positioning of UI Controls
      var mapSettings = ui.getControl('mapsettings');
      var zoom = ui.getControl('zoom');
      var scalebar = ui.getControl('scalebar');
      var panorama = ui.getControl('panorama');
      panorama.setAlignment('top-left');
      mapSettings.setAlignment('top-left');
      zoom.setAlignment('top-left');
      scalebar.setAlignment('top-left');
  
      