navigator.mediaDevices.getUserMedia({ audio: true })
    .then(stream => {
        console.log('Microphone access granted');
    })
    .catch(err => {
        console.error('Access denied:', err);
    });
