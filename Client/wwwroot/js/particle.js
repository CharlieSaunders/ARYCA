// Base code from Hima Vincent https://codemyui.com/square-emoji-shadow-and-line-particles-popper-effect-on-button-click/

function Particles(event, div) {
    let effect = event.target.dataset.type;
    let reference = div.id;
    let amount = 30;

    for (let i = 0; i < amount; i++) {
        createParticle(event.clientX, event.clientY + window.scrollY, effect, reference);
    }
}

function createParticle(x, y, type, div) {
    const particle = document.createElement('particle');
    document.getElementById(div).append(particle);
        
    let width = Math.floor(Math.random() * 30 + 8);
    let height = width;
    let destinationX = (Math.random() - 0.5) * 300;
    let destinationY = (Math.random() - 0.5) * 300;
    let rotation = Math.random() * 520;
    let delay = Math.random() * 200;

    switch (type) {
        case 'Square':
            particle.style.background = `hsl(${Math.random() * 90 + 270}, 70%, 60%)`;
            particle.style.border = '1px solid white';
            break;
        case 'Heart Emoji':
            particle.innerHTML = ['❤', '🧡', '💛', '💚', '💙', '💜', '🤎'][Math.floor(Math.random() * 7)];
            particle.style.fontSize = `${Math.random() * 24 + 10}px`;
            width = height = 'auto';
            break;
        case 'TradeProfitParticle':
            particle.innerHTML = ['💵', '🤑', '💲'][Math.floor(Math.random() * 3)];
            particle.style.fontSize = `${Math.random() * 24 + 10}px`;
            width = height = 'auto';
            break;
        case 'Firework':
            var color = `hsl(${Math.floor(Math.random() * (360 - 1 + 1) + 1) }, 70%, 50%)`;
            particle.style.boxShadow = `0 0 ${Math.floor(Math.random() * 10 + 10)}px ${color}`;
            particle.style.background = color;
            particle.style.borderRadius = '50%';
            width = height = Math.random() * 5 + 4;
            break;
    }

    particle.style.width = `${width}px`;
    particle.style.height = `${height}px`;
    const animation = particle.animate([
        {
            transform: `translate(-50%, -50%) translate(${x}px, ${y}px) rotate(0deg)`,
            opacity: 1
        },
        {
            transform: `translate(-50%, -50%) translate(${x + destinationX}px, ${y + destinationY}px) rotate(${rotation}deg)`,
            opacity: 0
        }
    ], {
        duration: Math.random() * 1000 + 5000,
        easing: 'cubic-bezier(0, .9, .57, 1)',
        delay: delay
    });
    animation.onfinish = removeParticle;
}
function removeParticle(e) {
    e.srcElement.effect.target.remove();
}