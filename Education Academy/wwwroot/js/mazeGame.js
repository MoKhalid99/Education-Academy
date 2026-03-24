var mazeGame = {
    canvas: null, ctx: null, player: { x: 50, y: 50, dir: 0 },
    init: function (id) {
        this.canvas = document.getElementById(id);
        this.ctx = this.canvas.getContext("2d");
        this.draw();
    },
    draw: function () {
        this.ctx.clearRect(0, 0, 600, 400);
        // رسم المتاهة تبسيط
        this.ctx.fillStyle = "#eee";
        this.ctx.fillRect(0, 0, 600, 400);

        // رسم الشخصية
        this.ctx.fillStyle = "#4db6ac";
        this.ctx.beginPath();
        this.ctx.arc(this.player.x, this.player.y, 15, 0, Math.PI * 2);
        this.ctx.fill();

        // رسم الهدف
        this.ctx.fillStyle = "gold";
        this.ctx.fillRect(550, 350, 40, 40);
    },
    run: async function (commands) {
        for (let cmd of commands) {
            if (cmd === "Forward") this.player.x += 50;
            if (cmd === "Left") this.player.y -= 50;
            if (cmd === "Right") this.player.y += 50;
            this.draw();
            await new Promise(r => setTimeout(r, 500)); // تأخير للحركة
        }
        this.checkWin();
    },
    checkWin: function () {
        if (this.player.x >= 550 && this.player.y >= 350) {
            alert("مبروك! لقد حللت المتاهة وحصلت على 50 XP");
            // هنا نرسل نداء لـ Blazor لتحديث XP في قاعدة البيانات
        }
    },
    reset: function () {
        this.player = { x: 50, y: 50, dir: 0 };
        this.draw();
    }
};