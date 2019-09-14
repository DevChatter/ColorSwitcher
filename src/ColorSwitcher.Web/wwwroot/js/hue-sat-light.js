"use strict";
class HueSatLight
{
    constructor(hue, saturation, light)
    {
        this.hue = hue;
        this.saturation = saturation;
        this.light = light;
    }
    getPerceivedBrightness()
    {
        let rgb = this.toRgb();
        let red = rgb[0];
        let green = rgb[1];
        let blue = rgb[2];
        return Math.sqrt(red * red * 0.241 + green * green * 0.691 + blue * blue * 0.068);
    }
    isBright()
    {
        return this.getPerceivedBrightness() > 0.5;
    }
    isDark()
    {
        return this.getPerceivedBrightness() < 0.5;
    }
    toRgb()
    {
        var red, green, blue;
        if (this.saturation == 0)
        {
            red = green = blue = this.light; // achromatic
        }
        else
        {
            function hue2rgb(p, q, t)
            {
                if (t < 0)
                    t += 1;
                if (t > 1)
                    t -= 1;
                if (t < 1 / 6)
                    return p + (q - p) * 6 * t;
                if (t < 1 / 2)
                    return q;
                if (t < 2 / 3)
                    return p + (q - p) * (2 / 3 - t) * 6;
                return p;
            }
            var q = this.light < 0.5 ? this.light * (1 + this.saturation) : this.light + this.saturation - this.light * this.saturation;
            var p = 2 * this.light - q;
            red = hue2rgb(p, q, this.hue + 1 / 3);
            green = hue2rgb(p, q, this.hue);
            blue = hue2rgb(p, q, this.hue - 1 / 3);
        }
        return [red, green, blue];
    }
    toHex()
    {
        let rgb = this.toRgb();
        let red = rgb[0];
        let green = rgb[1];
        let blue = rgb[2];
        function int2hex(value)
        {
            let hex = Math.round(value * 255).toString(16);
            if (hex.length == 1)
                hex = '0' + hex;
            return hex;
        }
        var redHex = int2hex(red);
        var greenHex = int2hex(green);
        var blueHex = int2hex(blue);
        return '#' + redHex + greenHex + blueHex;
    }
    static clone(hsl)
    {
        return new HueSatLight(hsl.hue, hsl.saturation, hsl.light);
    }
    static fromRGB(red, green, blue)
    {
        red /= 255;
        green /= 255;
        blue /= 255;
        let max = Math.max(red, green, blue);
        let min = Math.min(red, green, blue);
        let sum = max + min;
        let hue = 0, sat = 0, light = sum / 2;
        if (max == min)
        {
            hue = sat = 0; // gray scale
        }
        else
        {
            let dif = max - min;
            sat = light > 0.5 ? dif / (2 - sum) : dif / sum;
            switch (max)
            {
                case red:
                    hue = (green - blue) / dif + (green < blue ? 6 : 0);
                    break;
                case green:
                    hue = (blue - red) / dif + 2;
                    break;
                case blue:
                    hue = (red - green) / dif + 4;
                    break;
            }
            hue /= 6;
        }
        function inbounds(value)
        {
            return Math.max(0, Math.min(value, 1));
        }
        return new HueSatLight(inbounds(hue), inbounds(sat), inbounds(light));
    }
    static fromHex(hex)
    {
        // Expand shorthand form (e.g. "08f") to full form (e.g. "0088FF")
        var shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
        hex = hex.replace(shorthandRegex, function(m, r, g, b) {
            return r + r + g + g + b + b;
        });
        var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
        if (!result)
            return null;
        let red = parseInt(result[1], 16);
        let green = parseInt(result[2], 16);
        let blue = parseInt(result[3], 16);
        return HueSatLight.fromRGB(red, green, blue);
    }
}