--[[
    GD50
    Pokemon

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

FadeOutState = Class{__includes = BaseState}

function FadeOutState:init(color, time, onFadeComplete)
    self.opacity = 255
    self.r = color.r
    self.g = color.g
    self.b = color.b
    self.time = time

    Timer.tween(self.time, {
        [self] = {opacity = 0}
    })
    :finish(function()
        gStateStack:pop()
        onFadeComplete()
    end)
end

function FadeOutState:update(dt)

end

function FadeOutState:render()
    love.graphics.setColor(self.r / 255, self.g / 255, self.b / 255, self.opacity / 255)
    love.graphics.rectangle('fill', 0, 0, VIRTUAL_WIDTH, VIRTUAL_HEIGHT)

    love.graphics.setColor(255 / 255, 255 / 255, 255 / 255, 255 / 255)
end