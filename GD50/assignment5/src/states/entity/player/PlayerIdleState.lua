--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

PlayerIdleState = Class{__includes = EntityIdleState}

function PlayerIdleState:enter(params)
    -- render offset for spaced character sprite
    self.entity.offsetY = 5
    self.entity.offsetX = 0
end

function PlayerIdleState:update(dt)
    EntityIdleState.update(self, dt)
end

function PlayerIdleState:update(dt)
    if love.keyboard.isDown('left') or love.keyboard.isDown('right') or
       love.keyboard.isDown('up') or love.keyboard.isDown('down') then
        self.entity:changeState('walk')
    end

    if love.keyboard.wasPressed('space') then
        -- if player is carrying pot then no swinging sword
        if self.entity.pot == false then
            self.entity:changeState('swing-sword')
        end
    end

    -- check for pots after player presses f or throw if already carried
    if love.keyboard.wasPressed('f') then
        if self.entity.pot then
            Event.dispatch('throw-pot')
        else
            Event.dispatch('lift-pot')
        end
    end
end