--[[
    GD50
    Pokemon

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

BattleMenuState = Class{__includes = BaseState}

function BattleMenuState:init(battleState, levelUp, hI, aI, dI, sI)
    self.battleState = battleState

    -- flag for levelUp and stats
    self.levelUp = levelUp or false
    self.hI = hI
    self.aI = aI
    self.dI = dI
    self.sI = sI
    
    if not self.levelUp then
        self.battleMenu = Menu {
            x = VIRTUAL_WIDTH - 64,
            y = VIRTUAL_HEIGHT - 64,
            width = 64,
            height = 64,
            items = {
                {
                    text = 'Fight',
                    onSelect = function()
                        gStateStack:pop()
                        gStateStack:push(TakeTurnState(self.battleState))
                    end
                },
                {
                    text = 'Run',
                    onSelect = function()
                        gSounds['run']:play()
                        
                        -- pop battle menu
                        gStateStack:pop()

                        -- show a message saying they successfully ran, then fade in
                        -- and out back to the field automatically
                        gStateStack:push(BattleMessageState('You fled successfully!',
                            function() end), false)
                        Timer.after(0.5, function()
                            gStateStack:push(FadeInState({
                                r = 255, g = 255, b = 255
                            }, 1,
                            
                            -- pop message and battle state and add a fade to blend in the field
                            function()

                                -- resume field music
                                gSounds['field-music']:play()

                                -- pop message state
                                gStateStack:pop()

                                -- pop battle state
                                gStateStack:pop()

                                gStateStack:push(FadeOutState({
                                    r = 255, g = 255, b = 255
                                }, 1, function()
                                    -- do nothing after fade out ends
                                end))
                            end))
                        end)
                    end
                }
            }
        }
    else
        -- get local variables for stats before level
        local hp = self.battleState.player.party.pokemon[1].HP - self.hI
        local attack = self.battleState.player.party.pokemon[1].attack - self.aI
        local defense = self.battleState.player.party.pokemon[1].defense - self.dI
        local speed = self.battleState.player.party.pokemon[1].speed - self.sI
        -- create large center menu for display
        self.battleMenu = Menu {
            x = (VIRTUAL_WIDTH / 2) - 80,
            y = (VIRTUAL_HEIGHT / 2) - 80,
            width = 160,
            height = 160,
            levelUp = true,
            items = {
                {
                    text = 'HP: ' .. hp .. " + " .. self.hI .. " = " .. hp + self.hI
                },
                {
                    text = 'Atk: ' .. attack .. " + " .. self.aI .. " = " .. attack + self.aI
                },
                {
                    text = 'Def: ' .. defense .. " + " .. self.dI .. " = " .. defense + self.dI
                },
                {
                    text = 'Spd: ' .. speed .. " + " .. self.sI .. " = " .. speed + self.sI
                }
            }
        }
    end
end

function BattleMenuState:update(dt)
    self.battleMenu:update(dt)
end

function BattleMenuState:render()
    self.battleMenu:render()
end