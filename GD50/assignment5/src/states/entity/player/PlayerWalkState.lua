--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

PlayerWalkState = Class{__includes = EntityWalkState}

function PlayerWalkState:init(player, dungeon)
    self.entity = player
    self.dungeon = dungeon

    -- render offset for spaced character sprite
    self.entity.offsetY = 5
    self.entity.offsetX = 0

end

function PlayerWalkState:update(dt)
    if love.keyboard.isDown('left') then
        self.entity.direction = 'left'
        if self.entity.pot then
            self.entity:changeAnimation('pot-walk-left')
        else
            self.entity:changeAnimation('walk-left')
        end
    elseif love.keyboard.isDown('right') then
        self.entity.direction = 'right'
        if self.entity.pot then
            self.entity:changeAnimation('pot-walk-right')
        else
            self.entity:changeAnimation('walk-right')
        end
    elseif love.keyboard.isDown('up') then
        self.entity.direction = 'up'
        if self.entity.pot then
            self.entity:changeAnimation('pot-walk-up')
        else
            self.entity:changeAnimation('walk-up')
        end
    elseif love.keyboard.isDown('down') then
        self.entity.direction = 'down'
        if self.entity.pot then
            self.entity:changeAnimation('pot-walk-down')
        else
            self.entity:changeAnimation('walk-down')
        end
    else
        self.entity:changeState('idle')
    end

    if love.keyboard.wasPressed('space') then
        -- if player is carrying pot then no swinging sword
        if self.entity.pot == false then
            self.entity:changeState('swing-sword')
        end
    end

    -- lift and throw pot events
    if love.keyboard.wasPressed('f') then
        if self.entity.pot then
            Event.dispatch('throw-pot')
        else
            Event.dispatch('lift-pot')
        end
    end

    -- perform base collision detection against walls
    EntityWalkState.update(self, dt)  

    -- if we bumped something when checking collision, check any object collisions
    if self.bumped then
        if self.entity.direction == 'left' then
            
            -- temporarily adjust position
            self.entity.x = self.entity.x - PLAYER_WALK_SPEED * dt
            
            for k, doorway in pairs(self.dungeon.currentRoom.doorways) do
                if self.entity:collides(doorway) and doorway.open then

                    -- shift entity to center of door to avoid phasing through wall
                    self.entity.y = doorway.y + 4
                    Event.dispatch('shift-left')
                end
            end

            -- readjust
            self.entity.x = self.entity.x + PLAYER_WALK_SPEED * dt
        elseif self.entity.direction == 'right' then
            
            -- temporarily adjust position
            self.entity.x = self.entity.x + PLAYER_WALK_SPEED * dt
            
            for k, doorway in pairs(self.dungeon.currentRoom.doorways) do
                if self.entity:collides(doorway) and doorway.open then

                    -- shift entity to center of door to avoid phasing through wall
                    self.entity.y = doorway.y + 4
                    Event.dispatch('shift-right')
                end
            end

            -- readjust
            self.entity.x = self.entity.x - PLAYER_WALK_SPEED * dt
        elseif self.entity.direction == 'up' then
            
            -- temporarily adjust position
            self.entity.y = self.entity.y - PLAYER_WALK_SPEED * dt
            
            for k, doorway in pairs(self.dungeon.currentRoom.doorways) do
                if self.entity:collides(doorway) and doorway.open then

                    -- shift entity to center of door to avoid phasing through wall
                    self.entity.x = doorway.x + 8
                    Event.dispatch('shift-up')
                end
            end

            -- readjust
            self.entity.y = self.entity.y + PLAYER_WALK_SPEED * dt
        else
            
            -- temporarily adjust position
            self.entity.y = self.entity.y + PLAYER_WALK_SPEED * dt
            
            for k, doorway in pairs(self.dungeon.currentRoom.doorways) do
                if self.entity:collides(doorway) and doorway.open then

                    -- shift entity to center of door to avoid phasing through wall
                    self.entity.x = doorway.x + 8
                    Event.dispatch('shift-down')
                end
            end

            -- readjust
            self.entity.y = self.entity.y - PLAYER_WALK_SPEED * dt
        end
    end
end