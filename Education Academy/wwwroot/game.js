// تحميل صورة رأس الثعبان
const headImage = new Image();
headImage.src = 'snake_head.png';

//  منع المتصفح من تحريك الصفحة عند الضغط على الأسهم
window.addEventListener("keydown", function (e) {
    if (["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", "Space"].indexOf(e.code) > -1) {
        e.preventDefault();
    }
}, false);

//  الدالة الرئيسية لرسم اللعبة
window.drawSnakeGame = (snake, food, block) => {
    const canvas = document.getElementById('snakeCanvas');
    if (!canvas) return;
    const ctx = canvas.getContext('2d');

    // الخلفية سوداء
    ctx.fillStyle = "black";
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // رسم الحدود الزرقاء
    ctx.strokeStyle = "blue";
    ctx.lineWidth = 10;
    ctx.strokeRect(0, 0, canvas.width, canvas.height);

    // رسم الطعام
    if (food) {
        ctx.fillStyle = "red";
        ctx.fillRect(food.x, food.y, block, block);

        // تأثير بسيط للطعام
        ctx.strokeStyle = "white";
        ctx.lineWidth = 1;
        ctx.strokeRect(food.x, food.y, block, block);
    }

    // رسم الثعبان
    if (snake && snake.length > 0) {
        snake.forEach((part, index) => {
            if (index === 0) {
                // رسم الرأس
                if (headImage.complete && headImage.naturalWidth !== 0) {
                    ctx.drawImage(headImage, part.x, part.y, block, block);
                } else {
                    // إذا لم تتحمل الصورة بعد، ارسم مربعاً أصفر لتمييز الرأس
                    ctx.fillStyle = "yellow";
                    ctx.fillRect(part.x, part.y, block, block);
                }
            } else {
                // رسم جسم الثعبان
                ctx.fillStyle = "#00ff00"; // لون أخضر
                ctx.fillRect(part.x, part.y, block, block);

                // رسم حدود سوداء رقيقة حول كل قطعة لتمييز الجسم
                ctx.strokeStyle = "black";
                ctx.lineWidth = 1;
                ctx.strokeRect(part.x, part.y, block, block);
            }
        });
    }
};

// ملاحظة للتصحيح: في حال لم تظهر الصورة
headImage.onerror = function () {
    console.error("خطأ: لم يتم العثور على صورة Snakehead.png في مجلد wwwroot");
};