window.setAudioSource = (audioElement, source) => {
    audioElement.src = source;
};

window.playAudio = (audioElement) => {
    audioElement.play();
};

window.pauseAudio = (audioElement) => {
    audioElement.pause();
};

window.getAudioCurrentTime = (audioElement) => {
    return audioElement ? audioElement.currentTime || null : null;
};

window.getAudioDuration = (audioElement) => {
    return audioElement ? audioElement.duration || null : null;
};

window.setAudioVolume = (audioElement, volume) => {
    audioElement.volume = volume;
};

window.getElementWidth = (elementId) => {
    const element = document.getElementById(elementId);
    return element ? element.offsetWidth : 0;
};

window.setAudioCurrentTime = (audioElement, time) => {
    if (audioElement) {
        audioElement.currentTime = time;
    }
};

window.getBoundingClientRect = (elementId) => {
    const element = document.getElementById(elementId);
    if (!element) {
        console.error(`Element with ID "${elementId}" not found.`);
        return { left: 0, width: 0 };
    }
    return element.getBoundingClientRect();
};
