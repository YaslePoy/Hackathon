function getRandomEmoji() {
    // Диапазоны Unicode для эмодзи
    const ranges = [
        [0x1F600, 0x1F64F],
    ];

    // Выбираем случайный диапазон
    const range = ranges[Math.floor(Math.random() * ranges.length)];

    // Генерируем случайный код в выбранном диапазоне
    const codePoint = Math.floor(Math.random() * (range[1] - range[0] + 1)) + range[0];

    // Преобразуем код в эмодзи
    return String.fromCodePoint(codePoint);
}

// Добавляем эмодзи в футер
document.addEventListener('DOMContentLoaded', function() {
    const emojiSpan = document.getElementById('random-emoji');
    if (emojiSpan) {
        emojiSpan.textContent = getRandomEmoji();
    }
}); 